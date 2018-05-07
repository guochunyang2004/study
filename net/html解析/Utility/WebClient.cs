using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LeaveHelper.Core
{
    public class WebClient
    {
        private WebBrowser _Wb;

        public WebBrowser Wb
        {
            get { return _Wb; }
            set { _Wb = value; }
        }
        private string _LastUrl = "";
        private HtmlDocument _HtmlDoc;
        private static int _Count = 0;
        public WebClient()
        {
            _Wb = new WebBrowser();
            _Wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(_Wb_DocumentCompleted);


            
             
        }
        private void _Wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (_Wb.ReadyState < WebBrowserReadyState.Complete
                || _Wb.Url.ToString() == _LastUrl
                || e.Url.ToString() != _Wb.Url.ToString()) return;
            _LastUrl = _Wb.Url.ToString();
            _HtmlDoc = _Wb.Document;
            obj.Set();//继续线程
        }
        

        public void AddToControl(Control frm)
        {
            frm.Controls.Add(_Wb);
            _Wb.Location = new System.Drawing.Point(12,51);
            _Wb.MinimumSize = new System.Drawing.Size(20, 20);
            _Wb.Size = new System.Drawing.Size(686, 549);
            //_Wb.Dock = DockStyle.Fill;
            _Count++;
          
        }
        
        System.Threading.AutoResetEvent obj = new System.Threading.AutoResetEvent(false);
        public HtmlDocument GetPage(string url)
        {
            _Wb.Navigate(url, false);
            //obj.Reset(); obj.SetAccessControl(new System.Security.AccessControl.EventWaitHandleSecurity());    
            //while (obj.WaitOne(10, false) == false) { Application.DoEvents(); }
            
            return _Wb.Document;
        }
        /// <summary>
        /// 执行页面js事件并返回页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public HtmlDocument InvokeMemberById(string id, string methodName)
        {
            _LastUrl = "";
            //_Wb.Navigate(url, false);
            HtmlElement ele = HtmlHelper.GetElementById(_Wb.Document.All, id);
            if (ele != null)
            {
                ele.InvokeMember(methodName);
            }
            obj.Reset(); obj.SetAccessControl(new System.Security.AccessControl.EventWaitHandleSecurity());
            while (obj.WaitOne(10, false) == false) { Application.DoEvents(); }
            return _Wb.Document;
        }
        public HtmlDocument InvokeMember(HtmlElement ele, string methodName)
        {
            _LastUrl = "";            
            if (ele != null)
            {
                ele.InvokeMember(methodName);
            }
            obj.Reset(); obj.SetAccessControl(new System.Security.AccessControl.EventWaitHandleSecurity());
            while (obj.WaitOne(10, false) == false) { Application.DoEvents(); }
            return _Wb.Document;
        }

        public HtmlElement GetElementByTagKey(string key)
        {
            return HtmlHelper.GetElementByTagKey(_Wb.Document.All, key);
        }

        //public string GetText(ItemModel itemInfo)
        //{
        //    return "";
        //}
    }
}
