using System.Text;
using System.Text.RegularExpressions;

namespace App.Core
{
    public class HtmlHander
    {
        #region 相关正则表达式

        /// <summary>
        /// 去掉所有html标签
        /// </summary> 
        //private static readonly Regex FilterAll = new Regex(
        //@"(\[([^=]*)(=[^\]]*)?\][\s\S]*?\[/\1\])|(?<lj>(?=[^\u4E00-\u9FA5\uFE30-\uFFA0,."");])<a\s+[^>]*>[^<]{2,}</a>(?=[^\u4E00-\u9FA5\uFE30-\uFFA0,."");]))|(?<Style><style[\s\S]+?/style>)|(?<select><select[\s\S]+?/select>)|(?<Script><script[\s\S]*?/script>)|(?<Explein><\!\-\-[\s\S]*?\-\->)|(?<li><li(\s+[^>]+)?>[\s\S]*?/li>)|(?<Html></?\s*[^> ]+(\s*[^=>]+?=['""]?[^""']+?['""]?)*?[^\[<]*>)|(?<Other>&[a-zA-Z]+;)|(?<Other2>\#[a-z0-9]{6})|(?<Space>\s+)|(\&\#\d+\;)",
        private static readonly Regex FilterAll = new Regex(@"<[^>]*(>|$)",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase); //(?<Link><a[\s\S]*?</a>)|
        //(?<Style><style[\s\S]+?/style>)|(?<select><select[\s\S]+?/select>)|(?<Script><script[\s\S]*?/script>)|(?<Explein><\!\-\-[\s\S]*?\-\->)|(?<li><li(\s+[^>]+)?>[\s\S]*?/li>)|(?<Html></?\s*[^> ]+(\s*[^=>]+?=['""]?[^""']+?['""]?)*?[^\[<]*>)|(?<Other>&[a-zA-Z]+;)|(?<Other2>\#[a-z0-9]{6})|(?<Space>\s+)

        /// <summary>
        /// 找出title标签
        /// </summary>
        private static readonly Regex FindTitle = new Regex(
        @"<\s*/?title\s*>",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出title标签内容
        /// </summary>
        private static readonly Regex FindTitleContent = new Regex(
        @"<\s*/?title\s*>(?<Content>[\s\S]*?)<\s*/?title\s*>",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出h 和Strong标签
        /// </summary>
        private static readonly Regex FindHStrong = new Regex(
        @"<\s*/?h\s*>|<\s*/?strong\s*>",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出p 和br标签
        /// </summary>
        private static readonly Regex FindPB = new Regex(
        @"<\s*/?p\s*>|<\s*br\s*/?>|<\s*/?tr\s*>",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出nbsp标签
        /// </summary>
        private static readonly Regex FindNbsp = new Regex(
        @"&nbsp",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出结尾标签
        /// </summary>
        private static readonly Regex FindS = new Regex(
        @"(?<Content>[\s\S]*?)\$",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出是否为标准句
        /// </summary>
        private static readonly Regex IsSen = new Regex(
        @"[,.，。!！;；:：……？?《》“”""]",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出是否为垃圾句[strong][h]标签过多的
        /// </summary>
        private static readonly Regex IsWs = new Regex(
        @"\[\(h\)\]",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出是否为垃圾句冒号和·-过多的
        /// </summary>
        private static readonly Regex IsWsM = new Regex(
        @"\[·]|[－]|[：:]",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出是否为BBS特征
        /// </summary>
        private static readonly Regex IsBbsInfo = new Regex(
        @"第[^楼]{1,50}楼|Powered\s*/?by[\s\S]*?Dvbbs|Powered\s*/?by[\s\S]*?Discuz",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 取KEYWORD
        /// </summary>
        private static readonly Regex mKeyWord = new Regex(
        @"<meta\s*name\s*=\s*['""]?keywords['""]?\s*content\s*=\s*['""]?(?<KeyWords>[^'"">]*)['""]?[^>]*>|<meta\s*content\s*=\s*['""]?(?<KeyWords>[^'"">]*)['""]?\s*name\s*=\s*['""]?keywords['""]?\s*[^>]*>
", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 取DESCRIPTION
        /// </summary>
        private static readonly Regex mDescription = new Regex(
        @"<meta\s*name\s*=\s*['""]?description['""]?\s*content\s*=\s*['""]?(?<description>[^'"">]*)['""]?[^>]*>|<meta\s*content\s*=\s*['""]?(?<description>[^'"">]*)['""]?\s*name\s*=\s*['""]?description['""]?\s*[^>]*>
", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 取Tags
        /// </summary>
        private static readonly Regex mTag = new Regex(
        @"<meta\s*name\s*=\s*['""]?tagwords['""]?\s*content\s*=\s*['""]?(?<tagwords>[^'"">]*)['""]?[^>]*>|<meta\s*content\s*=\s*['""]?(?<tagwords>[^'"">]*)['""]?\s*name\s*=\s*['""]?tagwords['""]?\s*[^>]*>
", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出是否为垃圾句:后字符号过少，：号前无“说”字,:号后无"关于"
        /// </summary>
        private static readonly Regex IsWsMM = new Regex(
        @"^[^说\s]{0,8}?[:：].{0,10}$",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出spider写入的url标记
        /// </summary>
        private static readonly Regex txtUrl = new Regex(
        @"当前URL为:http://(?<URL>.*)",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        /// <summary>
        /// 找出spider写入的锚点描述标记
        /// </summary>
        private static readonly Regex txtDescription = new Regex(
        @"当前链接描述为:(?<Describe>.*)",
        RegexOptions.ExplicitCapture
        | RegexOptions.Multiline
        | RegexOptions.IgnoreCase);

        ///// <summary>
        ///// 取需要a标签
        ///// </summary>
        private static readonly Regex cleanFirst = new Regex(
         @"([\u4E00-\u9FA5]|[\uFE30-\uFFA0]|[,."");])(?<Robbish1><a\s+[^>]*>)[^<]{1,6}(?<Robbish2></a>)([\u4E00-\u9FA5]|[\uFE30-\uFFA0]|[,."");])", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        private static readonly Regex a = new Regex(
         "(?m)<a[^><]+href=(\"|')?(?<url>([^>\"'\\s)])+)(\"|')?[^>]*>(?<text>(\\w|\\W)*?)</a[^>]*>", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase);


        #endregion

        public static string GetStringByBeginEnd(string SourceStr, string BeginStr, string EndStr)
        {
            string html = "";
            if (!string.IsNullOrEmpty(BeginStr) && !string.IsNullOrEmpty(EndStr))
            {
                int beginPos = SourceStr.IndexOf(BeginStr) + BeginStr.Length;
                int endPos = SourceStr.IndexOf(EndStr, beginPos);
                if (endPos < beginPos) return html;
                html = SourceStr.Substring(beginPos, endPos - beginPos);
            }
            return html;
        }

        public static string GetStringByBeginEnd(string SourceStr, string BeginStr, string EndStr,bool isRegex)
        {
            string html = "";
           
            
            if (!string.IsNullOrEmpty(BeginStr) && !string.IsNullOrEmpty(EndStr))
            {
                int beginPos = 0;
                int endPos = 0;
                if (isRegex)
                {
                    Regex reg = new Regex(BeginStr);
                    Match m = reg.Match(SourceStr);
                    if (m.Success)
                    {
                        beginPos = m.Index + m.Length;
                    }
                    reg = new Regex(EndStr);
                    m = reg.Match(SourceStr,beginPos);
                    if (m.Success)
                    {
                        endPos = m.Index;
                    }
                }
                else
                {
                    beginPos = SourceStr.IndexOf(BeginStr) + BeginStr.Length;
                    endPos = SourceStr.IndexOf(EndStr, beginPos);
                }
                if (endPos < beginPos) return html;
                html = SourceStr.Substring(beginPos, endPos - beginPos);

            }
            return html;
        }


        public static string[] GetStringArray(string str, Regex reg)
        {
            MatchCollection matches = reg.Matches(str);
            string[] result = new string[matches.Count];
            int i=0;
            foreach(Match match in matches)
            {
                result[i] = match.Value;
                i++;
            } //System.Windows.Forms.MessageBox.Show(matches.Count.ToString());
            return result;
        }
        public static string[] GetStringArray(string str, string regStr)
        {
            Regex reg = new Regex(regStr,
                      RegexOptions.ExplicitCapture
                    | RegexOptions.Multiline
                    | RegexOptions.IgnoreCase);
            return GetStringArray(str, reg);
        }

        public static string GetString(string str, Regex reg)
        {
            MatchCollection matches = reg.Matches(str);
            StringBuilder result = new StringBuilder();
            foreach(Match match in matches)
            {
                result.Append(match.Value);
            }
            return result.ToString();

        }
        public static string GetString(string str, string regStr)
        {
            Regex reg = new Regex(regStr, 
                      RegexOptions.ExplicitCapture
                    | RegexOptions.Multiline
                    | RegexOptions.IgnoreCase);
            return GetString(str, reg);
        }
        public static string GetA(string str)
        {
            return GetString(str, a);
        }
        public static string Replace(string input, string regStr,string newStr)
        {
            Regex reg = new Regex(regStr);
            return reg.Replace(input, newStr);
        }
        public static string RemoveAllHtml(string str)    
        {                
            return FilterAll.Replace(str,"");    
        }
    }
}
