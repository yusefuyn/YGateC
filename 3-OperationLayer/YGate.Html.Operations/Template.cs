using Microsoft.AspNetCore.Components;

namespace YGate.Html.Operations
{
    public class Template
    {
        public Template(string Source)
        {
            this.Source = Source;
        }
        /// <summary>
        /// Başlangıç ve bitiş etiketi aynı olan <Baslangic></Baslangic>
        /// html obje içeriğini döndürür.
        /// </summary>
        /// <param name="Markup"></param>
        /// <returns></returns>
        public string ExtractMarkupContent(string Markup)
        {
            string StartMarkup = $"<{Markup}>";
            string EndMarkup = $"</{Markup}>";
            int Baslangic = Source.IndexOf(StartMarkup);
            int Son = Source.IndexOf(EndMarkup);
            if (Baslangic == -1 || Son == -1 || Baslangic > Son)
            {
                return string.Empty;
            }
            Baslangic += StartMarkup.Length;
            Son += EndMarkup.Length;
            int Uzunluk = Son - Baslangic - EndMarkup.Length;
            return Source.Substring(Baslangic, Uzunluk);
        }
        /// <summary>
        /// DataView markup kaynağını getirir.
        /// </summary>
        /// <returns></returns>
        public MarkupString DataView()
        {
            MarkupString returnedMarkup = new("");
            string source = ExtractMarkupContent("DataView");
            returnedMarkup = new(source);
            return returnedMarkup;
        }
        public MarkupString ChildView()
        {
            MarkupString returnedMarkup = new("");
            string source = ExtractMarkupContent("ChildView");
            returnedMarkup = new(source);
            return returnedMarkup;
        }
        public MarkupString ListView()
        {
            MarkupString returnedMarkup = new("");
            string source = ExtractMarkupContent("ListView");
            returnedMarkup = new(source);
            return returnedMarkup;
        }

        public MarkupString ChatView() {
            MarkupString returnedMarkup = new("");
            string source = ExtractMarkupContent("ChatView");
            returnedMarkup = new(source);
            return returnedMarkup;
        }
        public string Source { get; set; }
    }
}
