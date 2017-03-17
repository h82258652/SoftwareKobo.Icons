using System;
using Windows.ApplicationModel.DataTransfer;

namespace IconBrowser2.Services
{
    public class ClipboardService : IClipboardService
    {
        public void SetHtml(string text, string html)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            var content = new DataPackage();
            content.SetText(text);
            content.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(html));
            Clipboard.SetContent(content);
        }
    }
}