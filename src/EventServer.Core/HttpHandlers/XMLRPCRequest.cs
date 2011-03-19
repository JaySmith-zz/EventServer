using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Xml;
using EventServer.Core.HttpHandlers;

namespace EventServer.Core.Services
{
    internal class XMLRPCRequest
    {
        public XMLRPCRequest(HttpContext input)
        {
            string inputXML = ParseRequest(input);
            LoadXMLRequest(inputXML); // Loads Method Call and Associated Variables
        }

        private string _appKey;
        private string _blogID;
        private List<XmlNode> _inputParams;
        private MWAMediaObject _media;
        private string _methodName;
        private int _numberOfPosts;
        private MWAPage _page;
        private string _pageID;
        private string _password;
        private MWAPost _post;
        private string _postID;
        private bool _publish;
        private string _userName;

        public string MethodName
        {
            get { return _methodName; }
        }

        public string AppKey
        {
            get { return _appKey; }
        }

        public string BlogID
        {
            get { return _blogID; }
        }

        public MWAMediaObject MediaObject
        {
            get { return _media; }
        }

        public int NumberOfPosts
        {
            get { return _numberOfPosts; }
        }

        public string Password
        {
            get { return _password; }
        }

        public MWAPost Post
        {
            get { return _post; }
        }

        public MWAPage Page
        {
            get { return _page; }
        }

        public string PostID
        {
            get { return _postID; }
        }

        public string PageID
        {
            get { return _pageID; }
        }

        public bool Publish
        {
            get { return _publish; }
        }

        public string UserName
        {
            get { return _userName; }
        }

        private string ParseRequest(HttpContext context)
        {
            var buffer = new byte[context.Request.InputStream.Length];
            context.Request.InputStream.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer);
        }

        private void LoadXMLRequest(string xml)
        {
            var request = new XmlDocument();
            try
            {
                if (!(xml.StartsWith("<?xml") || xml.StartsWith("<method")))
                    xml = xml.Substring(xml.IndexOf("<?xml"));
                request.LoadXml(xml);
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("01", "Invalid XMLRPC Request. (" + ex.Message + ")");
            }

            // Method name is always first
            _methodName = request.DocumentElement.ChildNodes[0].InnerText;

            // Parameters are next (and last)
            _inputParams = new List<XmlNode>();
            foreach (XmlNode node in request.SelectNodes("/methodCall/params/param"))
                _inputParams.Add(node);

            // Determine what params are what by method name
            switch (_methodName)
            {
                case "metaWeblog.newPost":
                    _blogID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    _post = GetPost(_inputParams[3]);
                    if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
                        _publish = false;
                    else
                        _publish = true;
                    break;
                case "metaWeblog.editPost":
                    _postID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    _post = GetPost(_inputParams[3]);
                    if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
                        _publish = false;
                    else
                        _publish = true;
                    break;
                case "metaWeblog.getPost":
                    _postID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    break;
                case "metaWeblog.newMediaObject":
                    _blogID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    _media = GetMediaObject(_inputParams[3]);
                    break;
                case "metaWeblog.getCategories":
                case "wp.getAuthors":
                case "wp.getPageList":
                case "wp.getPages":
                case "wp.getTags":
                    _blogID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    break;
                case "metaWeblog.getRecentPosts":
                    _blogID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    _numberOfPosts = Int32.Parse(_inputParams[3].InnerText, CultureInfo.InvariantCulture);
                    break;
                case "blogger.getUsersBlogs":
                case "metaWeblog.getUsersBlogs":
                    _appKey = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    break;
                case "blogger.deletePost":
                    _appKey = _inputParams[0].InnerText;
                    _postID = _inputParams[1].InnerText;
                    _userName = _inputParams[2].InnerText;
                    _password = _inputParams[3].InnerText;
                    if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
                        _publish = false;
                    else
                        _publish = true;
                    break;
                case "blogger.getUserInfo":
                    _appKey = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    break;
                case "wp.newPage":
                    _blogID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    _page = GetPage(_inputParams[3]);
                    if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
                        _publish = false;
                    else
                        _publish = true;
                    break;
                case "wp.getPage":
                    _blogID = _inputParams[0].InnerText;
                    _pageID = _inputParams[1].InnerText;
                    _userName = _inputParams[2].InnerText;
                    _password = _inputParams[3].InnerText;
                    break;
                case "wp.editPage":
                    _blogID = _inputParams[0].InnerText;
                    _pageID = _inputParams[1].InnerText;
                    _userName = _inputParams[2].InnerText;
                    _password = _inputParams[3].InnerText;
                    _page = GetPage(_inputParams[4]);
                    if (_inputParams[5].InnerText == "0" || _inputParams[5].InnerText == "false")
                        _publish = false;
                    else
                        _publish = true;
                    break;
                case "wp.deletePage":
                    _blogID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    _pageID = _inputParams[3].InnerText;
                    break;
                default:
                    throw new MetaWeblogException("02", "Unknown Method. (" + _methodName + ")");
            }
        }

        private MWAPost GetPost(XmlNode node)
        {
            var temp = new MWAPost();
            var cats = new List<string>();
            var tags = new List<string>();

            // Require Title and Description
            try
            {
                temp.title = node.SelectSingleNode("value/struct/member[name='title']").LastChild.InnerText;
                temp.description = node.SelectSingleNode("value/struct/member[name='description']").LastChild.InnerText;
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("05",
                                              "Post Struct Element, Title or Description,  not Sent. (" + ex.Message +
                                              ")");
            }
            if (node.SelectSingleNode("value/struct/member[name='link']") == null)
                temp.link = "";
            else
                temp.link = node.SelectSingleNode("value/struct/member[name='link']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='mt_allow_comments']") == null)
                temp.commentPolicy = "";
            else
                temp.commentPolicy =
                    node.SelectSingleNode("value/struct/member[name='mt_allow_comments']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='mt_excerpt']") == null)
                temp.excerpt = "";
            else
                temp.excerpt = node.SelectSingleNode("value/struct/member[name='mt_excerpt']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='wp_slug']") == null)
                temp.slug = "";
            else
                temp.slug = node.SelectSingleNode("value/struct/member[name='wp_slug']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='wp_author_id']") == null)
                temp.author = "";
            else
                temp.author = node.SelectSingleNode("value/struct/member[name='wp_author_id']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='categories']") != null)
            {
                XmlNode categoryArray = node.SelectSingleNode("value/struct/member[name='categories']").LastChild;
                foreach (XmlNode catnode in categoryArray.SelectNodes("array/data/value/string"))
                {
                    cats.Add(catnode.InnerText);
                }
            }
            temp.categories = cats;

            // postDate has a few different names to worry about
            if (node.SelectSingleNode("value/struct/member[name='dateCreated']") != null)
            {
                try
                {
                    string tempDate =
                        node.SelectSingleNode("value/struct/member[name='dateCreated']").LastChild.InnerText;
                    temp.postDate = DateTime.ParseExact(tempDate, "yyyyMMdd'T'HH':'mm':'ss",
                                                        CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
                catch
                {
                    // Ignore PubDate Error
                }
            }
            else if (node.SelectSingleNode("value/struct/member[name='pubDate']") != null)
            {
                try
                {
                    string tempPubDate =
                        node.SelectSingleNode("value/struct/member[name='pubDate']").LastChild.InnerText;
                    temp.postDate = DateTime.ParseExact(tempPubDate, "yyyyMMdd'T'HH':'mm':'ss",
                                                        CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
                catch
                {
                    // Ignore PubDate Error
                }
            }

            // WLW tags implementation using mt_keywords
            if (node.SelectSingleNode("value/struct/member[name='mt_keywords']") != null)
            {
                string tagsList = node.SelectSingleNode("value/struct/member[name='mt_keywords']").LastChild.InnerText;
                foreach (string item in tagsList.Split(','))
                {
                    tags.Add(item.Trim().ToLowerInvariant());
                }
            }
            temp.tags = tags;

            return temp;
        }

        private MWAPage GetPage(XmlNode node)
        {
            var temp = new MWAPage();

            // Require Title and Description
            try
            {
                temp.title = node.SelectSingleNode("value/struct/member[name='title']").LastChild.InnerText;
                temp.description = node.SelectSingleNode("value/struct/member[name='description']").LastChild.InnerText;
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("06",
                                              "Page Struct Element, Title or Description,  not Sent. (" + ex.Message +
                                              ")");
            }
            if (node.SelectSingleNode("value/struct/member[name='link']") == null)
                temp.link = "";
            else
                temp.link = node.SelectSingleNode("value/struct/member[name='link']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='dateCreated']") != null)
            {
                try
                {
                    string tempDate =
                        node.SelectSingleNode("value/struct/member[name='dateCreated']").LastChild.InnerText;
                    temp.pageDate = DateTime.ParseExact(tempDate, "yyyyMMdd'T'HH':'mm':'ss",
                                                        CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
                catch
                {
                    // Ignore PubDate Error
                }
            }

            //Keywords
            if (node.SelectSingleNode("value/struct/member[name='mt_keywords']") == null)
                temp.mt_keywords = "";
            else
                temp.mt_keywords = node.SelectSingleNode("value/struct/member[name='mt_keywords']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='wp_page_parent_id']") != null)
                temp.pageParentID =
                    node.SelectSingleNode("value/struct/member[name='wp_page_parent_id']").LastChild.InnerText;

            return temp;
        }

        private MWAMediaObject GetMediaObject(XmlNode node)
        {
            var temp = new MWAMediaObject();
            temp.name = node.SelectSingleNode("value/struct/member[name='name']").LastChild.InnerText;
            if (node.SelectSingleNode("value/struct/member[name='type']") == null)
                temp.type = "notsent";
            else
                temp.type = node.SelectSingleNode("value/struct/member[name='type']").LastChild.InnerText;
            temp.bits =
                Convert.FromBase64String(node.SelectSingleNode("value/struct/member[name='bits']").LastChild.InnerText);

            return temp;
        }
    }
}