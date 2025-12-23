using System;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Services;
using StrmAssistant.Web.Api;
using StrmAssistant.Web.Helper;

namespace StrmAssistant.Web.Service
{
    [Unauthenticated]
    public class ShortcutMenuService : IService, IRequiresRequest
    {
        private readonly IHttpResultFactory _resultFactory;

        public ShortcutMenuService(IHttpResultFactory resultFactory)
        {
            _resultFactory = resultFactory;
        }

        public IRequest Request { get; set; }

        public object Get(GetStrmAssistantJs request)
        {
            var stream = ShortcutMenuHelper.StrmAssistantJs;
            if (stream == null)
            {
                 return _resultFactory.GetResult(ReadOnlySpan<char>.Empty, "application/x-javascript");
            }
            return _resultFactory.GetResult(Request,
                (ReadOnlyMemory<byte>)stream.GetBuffer(), "application/x-javascript");
        }

        public object Get(GetShortcutMenu request)
        {
            var content = ShortcutMenuHelper.ModifiedShortcutsString;
            if (string.IsNullOrEmpty(content))
            {
                // 如果初始化失败，返回空内容以避免 500 错误
                return _resultFactory.GetResult(ReadOnlySpan<char>.Empty, "application/x-javascript");
            }

            return _resultFactory.GetResult(content.AsSpan(),
                "application/x-javascript");
        }
    }
}
