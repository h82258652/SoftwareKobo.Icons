using System;
using Windows.ApplicationModel.DataTransfer;

namespace IconBrowser2.Services
{
    public class ClipboardService : IClipboardService
    {
        public void SetRawHtml(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            var content = new DataPackage();
            content.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(html));
            Clipboard.SetContent(content);
        }
    }
}