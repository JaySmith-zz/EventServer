using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace EventServer.Core.HttpHandlers
{
    internal class XMLRPCResponse
    {
        public XMLRPCResponse(string methodName)
        {
            _methodName = methodName;
            _blogs = new List<MWABlogInfo>();
            _categories = new List<MWACategory>();
            _keywords = new List<string>();
            _posts = new List<MWAPost>();
            _pages = new List<MWAPage>();
            _authors = new List<MWAAuthor>();
        }

        private string _methodName;
        private List<MWABlogInfo> _blogs;
        private List<MWACategory> _categories;
        private List<string> _keywords;
        private bool _completed;
        private MWAFault _fault;
        private MWAMediaInfo _mediaInfo;
        private MWAPost _post;
        private string _postID;
        private List<MWAPost> _posts;
        private List<MWAPage> _pages;
        private MWAPage _page;
        private string _pageID;
        private List<MWAAuthor> _authors;

        public List<MWAAuthor> Authors
        {
            get { return _authors; }
            set { _authors = value; }
        }

        public List<MWABlogInfo> Blogs
        {
            get { return _blogs; }
            set { _blogs = value; }
        }

        public List<MWACategory> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        public List<string> Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }

        public bool Completed
        {
            get { return _completed; }
            set { _completed = value; }
        }

        public MWAFault Fault
        {
            get { return _fault; }
            set { _fault = value; }
        }

        public MWAMediaInfo MediaInfo
        {
            get { return _mediaInfo; }
            set { _mediaInfo = value; }
        }

        public MWAPost Post
        {
            get { return _post; }
            set { _post = value; }
        }

        public string PostID
        {
            get { return _postID; }
            set { _postID = value; }
        }

        public string PageID
        {
            get { return _pageID; }
            set { _pageID = value; }
        }

        public List<MWAPost> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }

        public List<MWAPage> Pages
        {
            get { return _pages; }
            set { _pages = value; }
        }

        public MWAPage Page
        {
            get { return _page; }
            set { _page = value; }
        }

        public void Response(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            using (XmlTextWriter data = new XmlTextWriter(context.Response.OutputStream, System.Text.Encoding.UTF8))
            {
                data.Formatting = Formatting.Indented;
                data.WriteStartDocument();
                data.WriteStartElement("methodResponse");
                if (_methodName == "fault")
                    data.WriteStartElement("fault");
                else
                    data.WriteStartElement("params");

                switch (_methodName)
                {
                    case "metaWeblog.newPost":
                        WriteNewPost(data);
                        break;
                    case "metaWeblog.getPost":
                        WritePost(data);
                        break;
                    case "metaWeblog.newMediaObject":
                        WriteMediaInfo(data);
                        break;
                    case "metaWeblog.getCategories":
                        WriteGetCategories(data);
                        break;
                    case "metaWeblog.getRecentPosts":
                        WritePosts(data);
                        break;
                    case "blogger.getUsersBlogs":
                    case "metaWeblog.getUsersBlogs":
                        WriteGetUsersBlogs(data);
                        break;
                    case "metaWeblog.editPost":
                    case "blogger.deletePost":
                    case "wp.editPage":
                    case "wp.deletePage":
                        WriteBool(data);
                        break;
                    case "wp.newPage":
                        WriteNewPage(data);
                        break;
                    case "wp.getPage":
                        WritePage(data);
                        break;
                    case "wp.getPageList":
                        WriteShortPages(data);
                        break;
                    case "wp.getPages":
                        WritePages(data);
                        break;
                    case "wp.getAuthors":
                        WriteAuthors(data);
                        break;
                    case "wp.getTags":
                        WriteKeywords(data);
                        break;
                    case "fault":
                        WriteFault(data);
                        break;

                }

                data.WriteEndElement();
                data.WriteEndElement();
                data.WriteEndDocument();

            }
        }

        private void WriteFault(XmlTextWriter data)
        {
            data.WriteStartElement("value");
            data.WriteStartElement("struct");

            // faultCode
            data.WriteStartElement("member");
            data.WriteElementString("name", "faultCode");
            data.WriteElementString("value", _fault.faultCode);
            data.WriteEndElement();

            // faultString
            data.WriteStartElement("member");
            data.WriteElementString("name", "faultString");
            data.WriteElementString("value", _fault.faultString);
            data.WriteEndElement();

            data.WriteEndElement();
            data.WriteEndElement();

        }

        private void WriteBool(XmlTextWriter data)
        {
            string postValue = "0";
            if (_completed)
                postValue = "1";
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteElementString("boolean", postValue);
            data.WriteEndElement();
            data.WriteEndElement();
        }

        private void WriteGetCategories(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWACategory category in _categories)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // description
                data.WriteStartElement("member");
                data.WriteElementString("name", "description");
                data.WriteElementString("value", category.description);
                data.WriteEndElement();

                // categoryid
                data.WriteStartElement("member");
                data.WriteElementString("name", "categoryid");
                data.WriteElementString("value", category.id);
                data.WriteEndElement();

                // title
                data.WriteStartElement("member");
                data.WriteElementString("name", "title");
                data.WriteElementString("value", category.title);
                data.WriteEndElement();

                // htmlUrl 
                data.WriteStartElement("member");
                data.WriteElementString("name", "htmlUrl");
                data.WriteElementString("value", category.htmlUrl);
                data.WriteEndElement();

                // rssUrl
                data.WriteStartElement("member");
                data.WriteElementString("name", "rssUrl");
                data.WriteElementString("value", category.rssUrl);
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();

            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();

        }

        private void WriteKeywords(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (string keyword in _keywords)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // keywordName
                data.WriteStartElement("member");
                data.WriteElementString("name", "name");
                data.WriteElementString("value", keyword);
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();

            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();

        }

        private void WriteMediaInfo(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("struct");

            // url
            data.WriteStartElement("member");
            data.WriteElementString("name", "url");
            data.WriteStartElement("value");
            data.WriteElementString("string", _mediaInfo.url);
            data.WriteEndElement();
            data.WriteEndElement();

            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        private void WriteNewPost(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteElementString("string", _postID);
            data.WriteEndElement();
            data.WriteEndElement();
        }

        private void WriteNewPage(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteElementString("string", _pageID);
            data.WriteEndElement();
            data.WriteEndElement();
        }

        private void WritePost(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("struct");

            // postid
            data.WriteStartElement("member");
            data.WriteElementString("name", "postid");
            data.WriteStartElement("value");
            data.WriteElementString("string", _post.postID);
            data.WriteEndElement();
            data.WriteEndElement();

            // title
            data.WriteStartElement("member");
            data.WriteElementString("name", "title");
            data.WriteStartElement("value");
            data.WriteElementString("string", _post.title);
            data.WriteEndElement();
            data.WriteEndElement();

            // description
            data.WriteStartElement("member");
            data.WriteElementString("name", "description");
            data.WriteStartElement("value");
            data.WriteElementString("string", _post.description);
            data.WriteEndElement();
            data.WriteEndElement();

            // link
            data.WriteStartElement("member");
            data.WriteElementString("name", "link");
            data.WriteStartElement("value");
            data.WriteElementString("string", _post.link);
            data.WriteEndElement();
            data.WriteEndElement();

            // slug
            data.WriteStartElement("member");
            data.WriteElementString("name", "wp_slug");
            data.WriteStartElement("value");
            data.WriteElementString("string", _post.slug);
            data.WriteEndElement();
            data.WriteEndElement();

            // excerpt
            data.WriteStartElement("member");
            data.WriteElementString("name", "mt_excerpt");
            data.WriteStartElement("value");
            data.WriteElementString("string", _post.excerpt);
            data.WriteEndElement();
            data.WriteEndElement();

            // comment policy
            data.WriteStartElement("member");
            data.WriteElementString("name", "mt_allow_comments");
            data.WriteStartElement("value");
            data.WriteElementString("int", _post.commentPolicy);
            data.WriteEndElement();
            data.WriteEndElement();

            // dateCreated
            data.WriteStartElement("member");
            data.WriteElementString("name", "dateCreated");
            data.WriteStartElement("value");
            data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(_post.postDate));
            data.WriteEndElement();
            data.WriteEndElement();

            // publish
            data.WriteStartElement("member");
            data.WriteElementString("name", "publish");
            data.WriteStartElement("value");
            if (_post.publish)
                data.WriteElementString("boolean", "1");
            else
                data.WriteElementString("boolean", "0");
            data.WriteEndElement();
            data.WriteEndElement();

            // tags (mt_keywords)
            data.WriteStartElement("member");
            data.WriteElementString("name", "mt_keywords");
            data.WriteStartElement("value");
            string[] tags = new string[_post.tags.Count];
            for (int i = 0; i < _post.tags.Count; i++)
            {
                tags[i] = _post.tags[i];
            }
            string tagList = string.Join(",", tags);
            data.WriteElementString("string", tagList);
            data.WriteEndElement();
            data.WriteEndElement();

            // categories
            if (_post.categories.Count > 0)
            {
                data.WriteStartElement("member");
                data.WriteElementString("name", "categories");
                data.WriteStartElement("value");
                data.WriteStartElement("array");
                data.WriteStartElement("data");
                foreach (string cat in _post.categories)
                {
                    data.WriteStartElement("value");
                    data.WriteElementString("string", cat);
                    data.WriteEndElement();
                }
                data.WriteEndElement();
                data.WriteEndElement();
                data.WriteEndElement();
                data.WriteEndElement();
            }

            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        private void WritePage(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("struct");

            // pageid
            data.WriteStartElement("member");
            data.WriteElementString("name", "page_id");
            data.WriteStartElement("value");
            data.WriteElementString("string", _page.pageID);
            data.WriteEndElement();
            data.WriteEndElement();

            // title
            data.WriteStartElement("member");
            data.WriteElementString("name", "title");
            data.WriteStartElement("value");
            data.WriteElementString("string", _page.title);
            data.WriteEndElement();
            data.WriteEndElement();

            // description
            data.WriteStartElement("member");
            data.WriteElementString("name", "description");
            data.WriteStartElement("value");
            data.WriteElementString("string", _page.description);
            data.WriteEndElement();
            data.WriteEndElement();

            // link
            data.WriteStartElement("member");
            data.WriteElementString("name", "link");
            data.WriteStartElement("value");
            data.WriteElementString("string", _page.link);
            data.WriteEndElement();
            data.WriteEndElement();

            // mt_convert_breaks
            data.WriteStartElement("member");
            data.WriteElementString("name", "mt_convert_breaks");
            data.WriteStartElement("value");
            data.WriteElementString("string", "__default__");
            data.WriteEndElement();
            data.WriteEndElement();

            // dateCreated
            data.WriteStartElement("member");
            data.WriteElementString("name", "dateCreated");
            data.WriteStartElement("value");
            data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(Page.pageDate));
            data.WriteEndElement();
            data.WriteEndElement();

            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        private void WritePosts(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWAPost post in _posts)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // postid
                data.WriteStartElement("member");
                data.WriteElementString("name", "postid");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.postID);
                data.WriteEndElement();
                data.WriteEndElement();

                // dateCreated
                data.WriteStartElement("member");
                data.WriteElementString("name", "dateCreated");
                data.WriteStartElement("value");
                data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(post.postDate));
                data.WriteEndElement();
                data.WriteEndElement();

                // title
                data.WriteStartElement("member");
                data.WriteElementString("name", "title");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.title);
                data.WriteEndElement();
                data.WriteEndElement();

                // description
                data.WriteStartElement("member");
                data.WriteElementString("name", "description");
                data.WriteElementString("value", post.description);
                data.WriteEndElement();

                // link
                data.WriteStartElement("member");
                data.WriteElementString("name", "link");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.link);
                data.WriteEndElement();
                data.WriteEndElement();

                // slug
                data.WriteStartElement("member");
                data.WriteElementString("name", "wp_slug");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.slug);
                data.WriteEndElement();
                data.WriteEndElement();

                // excerpt
                data.WriteStartElement("member");
                data.WriteElementString("name", "mt_excerpt");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.excerpt);
                data.WriteEndElement();
                data.WriteEndElement();

                // comment policy
                data.WriteStartElement("member");
                data.WriteElementString("name", "mt_allow_comments");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.commentPolicy);
                data.WriteEndElement();
                data.WriteEndElement();

                // tags (mt_keywords)
                data.WriteStartElement("member");
                data.WriteElementString("name", "mt_keywords");
                data.WriteStartElement("value");
                string[] tags = new string[post.tags.Count];
                for (int i = 0; i < post.tags.Count; i++)
                {
                    tags[i] = post.tags[i];
                }
                string tagList = string.Join(",", tags);
                data.WriteElementString("string", tagList);
                data.WriteEndElement();
                data.WriteEndElement();

                // publish
                data.WriteStartElement("member");
                data.WriteElementString("name", "publish");
                data.WriteStartElement("value");
                if (post.publish)
                    data.WriteElementString("boolean", "1");
                else
                    data.WriteElementString("boolean", "0");
                data.WriteEndElement();
                data.WriteEndElement();

                // categories
                if (post.categories.Count > 0)
                {
                    data.WriteStartElement("member");
                    data.WriteElementString("name", "categories");
                    data.WriteStartElement("value");
                    data.WriteStartElement("array");
                    data.WriteStartElement("data");
                    foreach (string cat in post.categories)
                    {
                        data.WriteStartElement("value");
                        data.WriteElementString("string", cat);
                        data.WriteEndElement();
                    }
                    data.WriteEndElement();
                    data.WriteEndElement();
                    data.WriteEndElement();
                    data.WriteEndElement();
                }

                data.WriteEndElement();
                data.WriteEndElement();

            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();

        }

        private void WritePages(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWAPage page in _pages)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // pageid
                data.WriteStartElement("member");
                data.WriteElementString("name", "page_id");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.pageID);
                data.WriteEndElement();
                data.WriteEndElement();

                // title
                data.WriteStartElement("member");
                data.WriteElementString("name", "title");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.title);
                data.WriteEndElement();
                data.WriteEndElement();

                // description
                data.WriteStartElement("member");
                data.WriteElementString("name", "description");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.description);
                data.WriteEndElement();
                data.WriteEndElement();

                // link
                data.WriteStartElement("member");
                data.WriteElementString("name", "link");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.link);
                data.WriteEndElement();
                data.WriteEndElement();

                // mt_convert_breaks
                data.WriteStartElement("member");
                data.WriteElementString("name", "mt_convert_breaks");
                data.WriteStartElement("value");
                data.WriteElementString("string", "__default__");
                data.WriteEndElement();
                data.WriteEndElement();

                // dateCreated
                data.WriteStartElement("member");
                data.WriteElementString("name", "dateCreated");
                data.WriteStartElement("value");
                data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(page.pageDate));
                data.WriteEndElement();
                data.WriteEndElement();

                // page_parent_id
                if (page.pageParentID != null && page.pageParentID != "")
                {
                    data.WriteStartElement("member");
                    data.WriteElementString("name", "page_parent_id");
                    data.WriteStartElement("value");
                    data.WriteElementString("string", page.pageParentID);
                    data.WriteEndElement();
                    data.WriteEndElement();
                }

                data.WriteEndElement();
                data.WriteEndElement();

            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        private void WriteShortPages(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWAPage page in _pages)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // pageid
                data.WriteStartElement("member");
                data.WriteElementString("name", "page_id");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.pageID);
                data.WriteEndElement();
                data.WriteEndElement();

                // title
                data.WriteStartElement("member");
                data.WriteElementString("name", "page_title");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.title);
                data.WriteEndElement();
                data.WriteEndElement();

                // page_parent_id
                data.WriteStartElement("member");
                data.WriteElementString("name", "page_parent_id");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.pageParentID);
                data.WriteEndElement();
                data.WriteEndElement();

                // dateCreated
                data.WriteStartElement("member");
                data.WriteElementString("name", "dateCreated");
                data.WriteStartElement("value");
                data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(page.pageDate));
                data.WriteEndElement();
                data.WriteEndElement();

                // dateCreated gmt
                data.WriteStartElement("member");
                data.WriteElementString("name", "date_created_gmt");
                data.WriteStartElement("value");
                data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(page.pageDate));
                data.WriteEndElement();
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();

            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        private void WriteGetUsersBlogs(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWABlogInfo blog in _blogs)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // url
                data.WriteStartElement("member");
                data.WriteElementString("name", "url");
                data.WriteElementString("value", blog.url);
                data.WriteEndElement();

                // blogid
                data.WriteStartElement("member");
                data.WriteElementString("name", "blogid");
                data.WriteElementString("value", blog.blogID);
                data.WriteEndElement();

                // blogName
                data.WriteStartElement("member");
                data.WriteElementString("name", "blogName");
                data.WriteElementString("value", blog.blogName);
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();

            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();

        }

        private string ConvertDatetoISO8601(DateTime date)
        {
            string temp = date.Year.ToString() + date.Month.ToString().PadLeft(2, '0') + date.Day.ToString().PadLeft(2, '0') +
                          "T" + date.Hour.ToString().PadLeft(2, '0') + ":" + date.Minute.ToString().PadLeft(2, '0') + ":" + date.Second.ToString().PadLeft(2, '0');
            return temp;
        }

        private void WriteAuthors(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWAAuthor author in _authors)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // user id
                data.WriteStartElement("member");
                data.WriteElementString("name", "user_id");
                data.WriteElementString("value", author.user_id);
                data.WriteEndElement();

                // login
                data.WriteStartElement("member");
                data.WriteElementString("name", "user_login");
                data.WriteElementString("value", author.user_login);
                data.WriteEndElement();

                // display name 
                data.WriteStartElement("member");
                data.WriteElementString("name", "display_name");
                data.WriteElementString("value", author.display_name);
                data.WriteEndElement();

                // user email
                data.WriteStartElement("member");
                data.WriteElementString("name", "user_email");
                data.WriteElementString("value", author.user_email);
                data.WriteEndElement();

                // meta value
                data.WriteStartElement("member");
                data.WriteElementString("name", "meta_value");
                data.WriteElementString("value", author.meta_value);
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();

            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();

        }
    }
}