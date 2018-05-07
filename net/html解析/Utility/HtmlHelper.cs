using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LeaveHelper.Core
{
    public class HtmlHelper
    {

        #region GetElementById
        public static HtmlElement GetElementById(HtmlElementCollection ele, string value)
        {
            foreach (HtmlElement item in ele)
            {
                if (string.Compare(item.Id, value, true) == 0)
                {
                    return item;
                }
            }
            return null;
        }
        #endregion

        #region GetElementByName
        public static HtmlElement GetElementByName(HtmlElementCollection ele, string value)
        {
            foreach (HtmlElement item in ele)
            {
                if (string.Compare(item.Name, value, true) == 0)
                {
                    return item;
                }
            }
            return null;
        }
        #endregion

        #region GetElementByAttribute
        public static HtmlElement GetElementByAttribute(HtmlElementCollection ele, string name, string value)
        {
            if (name == "value")
            {
                foreach (HtmlElement item in ele)
                {
                    if (string.Compare(item.GetAttribute(name), value, true) == 0)
                    {
                        return item;
                    }
                }
            }
            else
            {
                string findStr = name + "=" + value;
                foreach (HtmlElement item in ele)
                {
                    //int start = item.OuterHtml.IndexOf("<");
                    int end = item.OuterHtml.IndexOf(">");
                    if (end > 2)
                    {
                        if (item.OuterHtml.Substring(2, end - 2).Replace("\\\"", "").IndexOf(findStr
                            , StringComparison.CurrentCultureIgnoreCase) > 0)
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region 属性包含
        /// <summary>
        /// 属性包含
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetElementIndexByAttributeContain(HtmlElementCollection ele, string value)
        {
            string findStr = value;
            int index = 0;
            foreach (HtmlElement item in ele)
            {
                //int start = item.OuterHtml.IndexOf("<");
                int end = item.OuterHtml.IndexOf(">");
                if (end > 2)
                {
                    if (item.OuterHtml.Substring(2, end - 2).Replace("\\\"", "").IndexOf(findStr
                        , StringComparison.CurrentCultureIgnoreCase) > 0)
                    {
                        return index;
                    }
                }
                index++;
            }
            return -1;
        }
        #endregion

        #region 获取元素By属性
        //获取元素By属性
        public static HtmlElement GetElementByClassName(HtmlElement ele, string value)
        {
            return GetElementByAttribute(ele.All, "class", value);
        }   


        //获取元素By属性
        public static HtmlElement GetElementByClassName(HtmlElementCollection ele, string value)
        {
            return GetElementByAttribute(ele, "class", value);
        }
        #endregion

        #region GetElementClassName
        public static string GetElementClassName(HtmlElement ele)
        {
            //IHTMLElement HTMLEle = (IHTMLElement)ele.DomElement;
            //return HTMLEle.className;
            string className = "";
            int end = ele.OuterHtml.IndexOf(">");
            if (end > 2)
            {
                int start = ele.OuterHtml.IndexOf("class="
                    , StringComparison.CurrentCultureIgnoreCase);
                if (start > 0)
                {
                    start += 6;
                    if (start < end)
                    {
                        string str = ele.OuterHtml.Substring(start, end - start);
                        className = str.Replace("\\\"", "").Replace(" ", "");
                        //for (int i = 0; i < str.Length; i++)
                        //{
                        //    if (str.Substring(i, 1) == " "
                        //        || str.Substring(i, 1) == ">")
                        //    {
                        //        className = str.Substring(0, i);
                        //    }
                        //}
                    }
                }
            }
            return className;
        }
        #endregion

        #region GetElementByKey
        public static HtmlElement GetElementByTagKey(HtmlElement ele,string key)
        {
            foreach (HtmlElement item in ele.All)
            {            
                if (item.OuterHtml.IndexOf(">")>0
                    && item.OuterHtml.IndexOf(key, 0, item.OuterHtml.IndexOf(">")
                    , StringComparison.OrdinalIgnoreCase) > 0)
                    return item;
            }
            return null;
        }
        #endregion

        public static HtmlElement GetElementByTagKey(HtmlElementCollection ele, string key)
        {
            foreach (HtmlElement item in ele)
            {
                if (item.OuterHtml.IndexOf(">") > 0)
                {
                    string strTag = item.OuterHtml.Substring(0, item.OuterHtml.IndexOf(">")).Replace("\"", "");
                    if(strTag.IndexOf(key.Replace("\"",""),StringComparison.OrdinalIgnoreCase)>0)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
