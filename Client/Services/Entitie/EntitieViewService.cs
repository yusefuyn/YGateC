using Microsoft.AspNetCore.Components;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;

namespace YGate.Client.Services.Entitie
{
    public class EntitieViewService : IEntitieViewService
    {
        private MarkupString CategoryHtmlTemplateAddValues(EntitieViewModel entitie, TemplateEnum TempType) => CategoryHtmlTemplateAddValues(entitie, entitie.HtmlTemplate.Template.ToString(), TempType);
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
                        Markup = "]>{" + entitieProperty.PropertyName + "}";
                        var replace = entitieProperty.PropertyValue;


                        if (entitieProperty.Type == Entities.BasedModel.PropertyValueType.ValueList)
                        {
                            if (!Grouped)
                            {
                                Template = Template.Replace(Markup, $"<customlistcomponent><listcomponent>{replace}</listcomponent>{Markup}</customlistcomponent>");
                                Grouped = true;
                                continue;
                            }
                            replace = $"<listcomponent>{replace}</listcomponent>{Markup}";
                            if (!MarkupToBeDeletedAfterTheReplaceOperation)
                                MarkupToBeDeletedAfterTheReplaceOperation = true;
                        }
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
                { "]>{GoLink}", PrepateTheLink(entitie) },
                { "]>{GoOwnerLink}", $"/Show/User/{entitie.OwnerGuid}" },
                { "]>{OwnerName}", entitie.OwnerName },
                { "]>{CreateDate}", entitie.SharedDateUTC.ToString() },
                { "]>{CategoryName}", entitie.CategoryName.ToString()}
            };

            foreach (var deger in ozelliklerDict)
            {
                Template = Template.Replace(deger.Key, deger.Value);
            }

            try
            {

                string ChildMarkup = "<ChildEntitieTemplate></ChildEntitieTemplate>";
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
            return CategoryHtmlTemplateAddValues(entitieViewModel, TemplateEnum.ListingView);
        }




    }
}
