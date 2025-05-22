using Microsoft.AspNetCore.Components;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;

namespace YGate.Client.Services.Entitie
{
    public class EntitieViewService : IEntitieViewService
    {
        private MarkupString CategoryHtmlTemplateAddValues(EntitieViewModel entitie, TemplateEnum TempType) => CategoryHtmlTemplateAddValues(entitie, entitie.HtmlTemplate.Template.ToString(), TempType);
        /// <summary>
        /// ]>{} Şeklinde olanları Üretir
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private string ReplaceMarkupCreate(string val)
            => "]>" + '{' + val + '}';
        /// <summary>
        /// <></> Şeklinde olanı üretir
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private string TemplateMarkupCreate(string val)
            => '<' + val + "></" + val + ">";
        private string TemplateMarkupCreateAndAddValue(string CreatedMarkup, string AddValue)
            => TemplateMarkupAddValue(AddValue, TemplateMarkupCreate(CreatedMarkup));
        private string TemplateMarkupAddValue(string val, string TemplateMarkup)
            => TemplateMarkup.Replace("><", $">{val}<");
        private MarkupString CategoryHtmlTemplateAddValues(EntitieViewModel entitie, string temp, TemplateEnum TempType)
        {
            if (entitie == null || string.IsNullOrEmpty(temp))
                return new("");

            string Template = temp.ToString();

            var groups = entitie.Values// Aynı liste öğelerini gruplaması ve tekilleri tekil tutması için grupluyoruz. PropertyDBGuid ile
                .GroupBy(v => v.CategoryTemplateGuid)
                .ToDictionary(g => g.Key, g => g.ToList());


            foreach (var group in groups)
            {
                bool Grouped = false; // Bu öğelerin grup etiketi başlatılmışmı.
                bool MarkupToBeDeletedAfterTheReplaceOperation = false; // Bu öğe replace'den sonra silinecek bir markupa sahipmi.
                string Markup = ""; // Markup.


                try
                {

                    foreach (var entitieProperty in group.Value)
                    {
                        Markup = ReplaceMarkupCreate(entitieProperty.PropertyName);
                        string replace = entitieProperty.PropertyValue.ToString();


                        if (entitieProperty.Type == PropertyValueType.ValueList)
                        {
                            if (!Grouped)
                            {
                                Template = Template.Replace(Markup, TemplateMarkupCreateAndAddValue("customlistcomponent", TemplateMarkupCreateAndAddValue("listcomponent", replace) + Markup));
                                Grouped = true;
                                continue;
                            }
                            replace = TemplateMarkupCreateAndAddValue("listcomponent", replace) + Markup;
                            Template = Template.Replace(Markup, replace);
                            if (!MarkupToBeDeletedAfterTheReplaceOperation)
                                MarkupToBeDeletedAfterTheReplaceOperation = true;
                        }
                        else if (entitieProperty.Type == PropertyValueType.Combos)
                        {
                            if (!Grouped)
                            {
                                Template = Template.Replace(Markup, TemplateMarkupCreateAndAddValue("customlistcomponent", TemplateMarkupCreateAndAddValue("combocomponent", replace) + Markup));
                                Grouped = true;
                                continue;
                            }
                            replace = TemplateMarkupCreateAndAddValue("combocomponent", replace) + Markup;
                            Template = Template.Replace(Markup, replace);
                            if (!MarkupToBeDeletedAfterTheReplaceOperation)
                                MarkupToBeDeletedAfterTheReplaceOperation = true;
                        }
                        else
                            Template = Template.Replace(Markup, replace);
                    }



                    if (MarkupToBeDeletedAfterTheReplaceOperation)
                        Template = Template.Replace(Markup, "");
                }
                catch (Exception ex)
                {

                }



            }

            var ozelliklerDict = new System.Collections.Generic.Dictionary<string, string>
            {
                { ReplaceMarkupCreate("GoLink"), PrepateTheLink(entitie) },
                { ReplaceMarkupCreate("GoOwnerLink"), $"/Show/User/{entitie.CreatorGuid}" },
                { ReplaceMarkupCreate("OwnerName"), entitie.OwnerName },
                { ReplaceMarkupCreate("CreateDate"), entitie.SharedDateUTC.ToString() },
                { ReplaceMarkupCreate("CategoryName"), entitie.CategoryName.ToString()}
            };

            foreach (var deger in ozelliklerDict)
            {
                Template = Template.Replace(deger.Key, deger.Value);
            }

            try
            {

                string ChildMarkup = TemplateMarkupCreate("ChildEntitieTemplate");
                string ChildTemplates = "";
                foreach (var childItem in entitie.ChildEntitie)
                {
                    if (childItem.HtmlTemplate == null || string.IsNullOrEmpty(childItem.HtmlTemplate.Template))
                        continue;

                    ChildTemplates += CategoryHtmlTemplateAddValues(childItem, TempType).Value.ToString();
                }

                ChildTemplates = $"{ChildTemplates.ToString()}";
                Template = Template.Replace(ChildMarkup, ChildTemplates);
            }
            catch (Exception ex)
            {

            }


            MarkupString markupString = new MarkupString();
            YGate.Html.Operations.Template template = new(Template);
            switch (TempType)
            {
                case TemplateEnum.DataView:
                    markupString = template.DataView();
                    break;
                case TemplateEnum.ListingView:
                    markupString = template.ListView();
                    break;
                case TemplateEnum.ChildView:
                    markupString = template.ChildView();
                    break;

                case TemplateEnum.ListingPage:
                    markupString = template.ListPage();
                    break;
            }

            return markupString;
        }
        private string PrepateTheLink(EntitieViewModel entitie)
        {
            string returnedLink = "";
            returnedLink += $"{entitie.CategoryName}-";
            foreach (var item in entitie.Values.Where(xd => xd.Seo == true))
                returnedLink += item.PropertyValue.ToString();
            returnedLink += $"-id-{entitie.Id.ToString().PadLeft(8, '0')}";
            returnedLink = $"/Show/Entitie/{TurkceKarakterleriDegistir(returnedLink)}";
            return returnedLink;
        }
        private string TurkceKarakterleriDegistir(string deger)
        {
            string returned = deger.ToLower();
            Dictionary<char, string> karakters = new() {
                { 'ş', "s" },
                { 'ğ', "g" },
                { 'ü', "u" },
                { 'ç', "c" },
                { 'ı', "i" },
                { 'ö', "o" },
                { ' ', "-" },
                { '.', "" }
            };
            foreach (var item in karakters)
                returned = returned.Replace(item.Key.ToString(), item.Value);
            return returned;
        }
        private enum TemplateEnum
        {
            DataView, // Verinin gösterileceği sayfadaki template'i
            ListingView, // Veri listelenirken gösterilecek template'i
            ListingPage, // Verinin liste template'ini içine alan görünüm.
            ChildView, // Veri başka bir verinin alt öğesi ise gösterilecek template'i
        }
        public MarkupString GetChildView(EntitieViewModel entitieViewModel)
        {
            return CategoryHtmlTemplateAddValues(entitieViewModel, TemplateEnum.ChildView);
        }
        public MarkupString GetDataView(EntitieViewModel entitieViewModel)
        {
            return CategoryHtmlTemplateAddValues(entitieViewModel, TemplateEnum.DataView);

        }
        public MarkupString GetListView(EntitieViewModel entitieViewModel)
        {
            if (entitieViewModel.HtmlTemplate == null)
                entitieViewModel.HtmlTemplate = new()
                {
                    DBGuid = "",
                    Category = new CategoryViewModel() { },
                    CategoryGuid = "",
                    CreatorGuid = "",
                    Template = $"<DataView><p style='color:red;'>Bu objenin {entitieViewModel.DBGuid.ToString()} görseli oluşturulmamış. !</p></DataView>" +
                    $"<ListView><p style='color:red;'>Bu objenin {entitieViewModel.DBGuid.ToString()} görseli oluşturulmamış. !</p></ListView>" +
                    $"<ChildView><p style='color:red;'>Bu objenin {entitieViewModel.DBGuid.ToString()} görseli oluşturulmamış. !</p></ChildView>" +
                    $"<ChatView><p style='color:red;'>Bu objenin {entitieViewModel.DBGuid.ToString()} görseli oluşturulmamış. !</p></ChatView>"

                };
            return CategoryHtmlTemplateAddValues(entitieViewModel, TemplateEnum.ListingView);
        }
        public MarkupString GetCreateView(CategoryViewModel categoryTemplateViewModel)
        {
            var page = new YGate.Client.Pages.Entities.Add();
            return RenderFragmentToMarkupString(page.GetCategoryTemplate(categoryTemplateViewModel));
        }
        private MarkupString RenderFragmentToMarkupString(RenderFragment fragment)
        {
            // RenderTreeBuilder kullanarak RenderFragment içeriğini HTML olarak render etme
            var sb = new System.Text.StringBuilder();
            var writer = new System.IO.StringWriter(sb);

            // Burada RenderTreeBuilder kullanarak HTML içeriğini yakalıyoruz
            var builder = new Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder();
            fragment(builder);

            // RenderTreeBuilder'dan HTML string oluşturmak
            builder.CloseComponent();

            // Sonuç olarak string'i MarkupString'e dönüştürme
            return new MarkupString(sb.ToString());
        }
        public MarkupString GetListPage(EntitieViewModel PageViewModel, List<EntitieViewModel> ListEntitieViewModel)
        {
            MarkupString ListView = new("");
            foreach (EntitieViewModel item in ListEntitieViewModel)
            {
                MarkupString ob = GetListView(item);
                ListView = new(ListView.Value.ToString() + ob.Value.ToString());
            }
            MarkupString ListPage = CategoryHtmlTemplateAddValues(PageViewModel, TemplateEnum.ListingPage);
            ListPage = new(ListPage.Value.Replace(TemplateMarkupCreate("IListView"), ListView.Value.ToString()));
            return ListPage;
        }
        public MarkupString GetView(DynamicPageDynamicParameter dynamicPageDynamicParameter)
        {
            string Index = "";
            Index = dynamicPageDynamicParameter.PageSource.ToString();
            dynamicPageDynamicParameter.Parameters.ForEach(xd => Index = Index.Replace(xd.ToString(), xd.Value));
            return new MarkupString(Index);
        }
        public MarkupString GetView(string source)
        {
            return new(source);
        }
    }
}
