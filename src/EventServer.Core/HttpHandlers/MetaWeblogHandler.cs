using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using EventServer.Core.Domain;
using EventServer.Core.Services;

namespace EventServer.Core.HttpHandlers
{
    internal class MetaWeblogHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string rootUrl = HostingEnvironment.MapPath("~/");
                var input = new XMLRPCRequest(context);
                var output = new XMLRPCResponse(input.MethodName);

                switch (input.MethodName)
                {
                    case "blogger.getUsersBlogs":
                    case "metaWeblog.getUsersBlogs":
                        output.Blogs = GetUserBlogs(input.AppKey, input.UserName, input.Password, rootUrl);
                        break;
                    case "metaWeblog.newPost":
                        output.PostID = NewPost(input.BlogID, input.UserName, input.Password, input.Post, input.Publish);
                        break;
                    case "metaWeblog.editPost":
                        output.Completed = EditPost(input.PostID, input.UserName, input.Password, input.Post, input.Publish);
                        break;
                    case "metaWeblog.getPost":
                        output.Post = GetPost(input.PostID, input.UserName, input.Password);
                        break;
                    case "metaWeblog.newMediaObject":
                        output.MediaInfo = NewMediaObject(input.BlogID, input.UserName, input.Password, input.MediaObject, context);
                        break;
                    case "metaWeblog.getCategories":
                        output.Categories = GetCategories(input.BlogID, input.UserName, input.Password, rootUrl);
                        break;
                    case "metaWeblog.getRecentPosts":
                        output.Posts = GetRecentPosts(input.BlogID, input.UserName, input.Password, input.NumberOfPosts);
                        break;
                    case "blogger.deletePost":
                        output.Completed = DeletePost(input.AppKey, input.PostID, input.UserName, input.Password, input.Publish);
                        break;
                    case "blogger.getUserInfo":
                        //Not implemented.  Not planned.
                        throw new MetaWeblogException("10", "The method GetUserInfo is not implemented.");
                    case "wp.newPage":
                        output.PageID = NewPage(input.BlogID, input.UserName, input.Password, input.Page, input.Publish);
                        break;
                    case "wp.getPageList":
                    case "wp.getPages":
                        output.Pages = GetPages(input.BlogID, input.UserName, input.Password);
                        break;
                    case "wp.getPage":
                        output.Page = GetPage(input.BlogID, input.PageID, input.UserName, input.Password);
                        break;
                    case "wp.editPage":
                        output.Completed = EditPage(input.BlogID, input.PageID, input.UserName, input.Password, input.Page, input.Publish);
                        break;
                    case "wp.deletePage":
                        output.Completed = DeletePage(input.BlogID, input.PageID, input.UserName, input.Password);
                        break;
                    case "wp.getAuthors":
                        output.Authors = GetAuthors(input.BlogID, input.UserName, input.Password);
                        break;
                    case "wp.getTags":
                        output.Keywords = GetKeywords(input.BlogID, input.UserName, input.Password);
                        break;
                }

                output.Response(context);
            }
            catch (MetaWeblogException mex)
            {
                XMLRPCResponse output = new XMLRPCResponse("fault");
                MWAFault fault = new MWAFault();
                fault.faultCode = mex.Code;
                fault.faultString = mex.Message;
                output.Fault = fault;
                output.Response(context);
            }
            catch (Exception ex)
            {
                XMLRPCResponse output = new XMLRPCResponse("fault");
                MWAFault fault = new MWAFault();
                fault.faultCode = "0";
                fault.faultString = ex.Message;
                output.Fault = fault;
                output.Response(context);
            }
        }

        internal string NewPost(string blogId, string userName, string password, MWAPost sentPost, bool publish)
        {
            ValidateRequest(userName, password);

            var repository = Ioc.Resolve<IRepository>();

            var user = repository.Find<UserProfile>().GetBy(string.IsNullOrEmpty(sentPost.author) ? userName : sentPost.author);
            var post = new Post(user.Name, sentPost.title, sentPost.description, sentPost.excerpt, sentPost.slug, publish);

            foreach (string item in sentPost.categories.Where(x => x != null && x.Trim() != ""))
                post.Categories.Add(new Category(item.Trim(), ""));

            foreach (string item in sentPost.tags.Where(x => x != null && x.Trim() != ""))
                post.Tags.Add(item.Trim());

            if (sentPost.postDate != DateTime.MinValue)
                post.DateCreated = sentPost.postDate;

            repository.Save(post);

            return post.Id.ToString();
        }

        internal bool EditPost(string postID, string userName, string password, MWAPost sentPost, bool publish)
        {
            ValidateRequest(userName, password);

            var repository = Ioc.Resolve<IRepository>();

            var user = repository.Find<UserProfile>().GetBy(string.IsNullOrEmpty(sentPost.author) ? userName : sentPost.author);
            var post = repository.Get<Post>(postID.ToInt());

            post.Author = user.Name;
            post.Title = sentPost.title;
            post.Content = sentPost.description;
            post.IsPublished = publish;
            post.Slug = sentPost.slug;
            post.Description = sentPost.excerpt;

            post.Categories.Clear();
            foreach (string item in sentPost.categories.Where(x => x != null && x.Trim() != ""))
                post.Categories.Add(new Category(item.Trim(), ""));

            post.Tags.Clear();
            foreach (string item in sentPost.tags.Where(x => x != null && x.Trim() != ""))
                post.Tags.Add(item.Trim());

            if (sentPost.postDate != DateTime.MinValue)
                post.DateCreated = sentPost.postDate;

            repository.Save(post);

            return true;
        }

        internal MWAPost GetPost(string postID, string userName, string password)
        {
            ValidateRequest(userName, password);

            MWAPost sendPost = new MWAPost();

            var repository = Ioc.Resolve<IRepository>();
            Post post = repository.Get<Post>(x => x.Id == postID.ToInt());

            sendPost.postID = post.Id.ToString();
            sendPost.postDate = post.DateCreated;
            sendPost.title = post.Title;
            sendPost.description = post.Content;

            //TODO: ????
            //sendPost.link = post.AbsoluteLink.AbsoluteUri;
            sendPost.slug = post.Slug;
            sendPost.excerpt = post.Description;

            sendPost.publish = post.IsPublished;

            var categories = new List<string>();
            for (int i = 0; i < post.Categories.Count; i++)
            {
                var category = repository.Get<Category>(x => x.Id == post.Categories[i].Id);
                categories.Add(category.Title);
            }
            sendPost.categories = categories;

            List<string> tags = new List<string>();
            for (int i = 0; i < post.Tags.Count; i++)
                tags.Add(post.Tags[i]);

            sendPost.tags = tags;

            return sendPost;
        }

        internal MWAMediaInfo NewMediaObject(string blogID, string userName, string password, MWAMediaObject mediaObject, HttpContext request)
        {
            ValidateRequest(userName, password);

            MWAMediaInfo mediaInfo = new MWAMediaInfo();

            string saveFolder = HostingEnvironment.MapPath("~/App_Data/Files");
            //saveFolder = Settings.Instance.FileDataStorePath;
            string fileName = mediaObject.name;
            string mediaFolder = "";

            // Check/Create Folders & Fix fileName
            if (mediaObject.name.LastIndexOf('/') > -1)
            {
                mediaFolder = mediaObject.name.Substring(0, mediaObject.name.LastIndexOf('/'));
                saveFolder += mediaFolder;
                mediaFolder += "/";
                saveFolder = saveFolder.Replace('/', Path.DirectorySeparatorChar);
                fileName = mediaObject.name.Substring(mediaObject.name.LastIndexOf('/') + 1);
            }
            else
            {
                if (saveFolder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    saveFolder = saveFolder.Substring(0, saveFolder.Length - 1);
            }
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);
            saveFolder += Path.DirectorySeparatorChar;

            if (File.Exists(saveFolder + fileName))
            {
                // Find unique fileName
                for (int count = 1; count < 30000; count++)
                {
                    string tempFileName = fileName.Insert(fileName.LastIndexOf('.'), "_" + count);
                    if (!File.Exists(saveFolder + tempFileName))
                    {
                        fileName = tempFileName;
                        break;
                    }
                }
            }

            // Save File
            FileStream fs = new FileStream(saveFolder + fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(mediaObject.bits);
            bw.Close();

            // Set Url
            //TODO: ???? Jay
            string rootUrl = ""; // Utils.AbsoluteWebRoot.ToString();
            //if (BlogSettings.Instance.RequireSSLMetaWeblogAPI)
            //rootUrl = rootUrl.Replace("https://", "http://");

            string mediaType = mediaObject.type;
            if (mediaType.IndexOf('/') > -1)
                mediaType = mediaType.Substring(0, mediaType.IndexOf('/'));
            switch (mediaType)
            {
                case "image":
                case "notsent": // If there wasn't a type, let's pretend it is an image.  (Thanks Zoundry.  This is for you.)
                    rootUrl += "image.axd?picture=";
                    break;
                default:
                    rootUrl += "file.axd?file=";
                    break;
            }

            mediaInfo.url = rootUrl + mediaFolder + fileName;
            return mediaInfo;
        }

        internal List<MWACategory> GetCategories(string blogID, string userName, string password, string rootUrl)
        {
            List<MWACategory> categories = new List<MWACategory>();

            ValidateRequest(userName, password);

            var repository = Ioc.Resolve<IRepository>();
            var allCategories = repository.Find<Category>();

            foreach (Category category in allCategories)
            {
                MWACategory temp = new MWACategory();
                temp.title = category.Title;
                temp.description = category.Title; //cat.Description;
                //TODO: What is this for???? Jay
                //temp.htmlUrl = rootUrl + "category/" + category.Title + ".aspx";
                //temp.rssUrl = rootUrl + "category/syndication.axd?category=" + category.Id;
                categories.Add(temp);
            }

            return categories;
        }

        internal List<string> GetKeywords(string blogID, string userName, string password)
        {
            var keywords = new List<string>();

            ValidateRequest(userName, password);

            var repository = Ioc.Resolve<IRepository>();
            var posts = repository.Find<Post>();

            foreach (Post post in posts)
            {
                if (!post.IsPublished) continue;
                foreach (string tag in post.Tags)
                    if (!keywords.Contains(tag)) keywords.Add(tag);
            }

            keywords.Sort();

            return keywords;
        }

        internal List<MWAPost> GetRecentPosts(string blogID, string userName, string password, int numberOfPosts)
        {
            ValidateRequest(userName, password);

            return Ioc.Resolve<IRepository>()
                .Find<Post>()
                .OrderByDescending(x => x.Id)
                .Take(numberOfPosts)
                .Select(post => new MWAPost
                    {
                        postID = post.Id.ToString(),
                        postDate = post.DateCreated,
                        title = post.Title,
                        description = post.Content,
                        slug = post.Slug,
                        excerpt = post.Description,
                        publish = post.IsPublished,
                        author = post.Author,
                        tags = post.Tags,
                        categories = new List<string>(),
                        commentPolicy = "", //TODO:
                        link = "", //TODO:
                    })
                .ToList();
        }

        internal List<MWABlogInfo> GetUserBlogs(string appKey, string userName, string password, string rootUrl)
        {
            var blogs = new List<MWABlogInfo>();

            ValidateRequest(userName, password);

            var temp = new MWABlogInfo { url = rootUrl, blogID = "1000", blogName = "EventServer" };
            blogs.Add(temp);

            return blogs;
        }

        internal bool DeletePost(string appKey, string postID, string userName, string password, bool publish)
        {
            ValidateRequest(userName, password);
            try
            {
                var repository = Ioc.Resolve<IRepository>();
                var post = repository.Get<Post>(x => x.Id == postID.ToInt());
                repository.Delete(post);
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("12", "DeletePost failed.  Error: " + ex.Message);
            }

            return true;
        }

        /// <summary>
        /// wp.newPage
        /// </summary>
        /// <param name="blogID">blogID in string format</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="mPage"></param>
        /// <param name="publish"></param>
        /// <returns></returns>
        internal string NewPage(string blogID, string userName, string password, MWAPage mPage, bool publish)
        {
            ValidateRequest(userName, password);

            var repository = Ioc.Resolve<IRepository>();

            var page = new Page();

            page.Title = mPage.title;
            page.Content = mPage.description;
            page.Description = ""; // Can not be set from WLW
            page.Keywords = mPage.mt_keywords;
            if (mPage.pageDate != new DateTime())
                page.DateCreated = mPage.pageDate;
            page.ShowInList = publish;
            page.IsPublished = publish;

            repository.Save(page);

            return page.Id.ToString();
        }

        /// <summary>
        /// wp.getPages
        /// </summary>
        /// <param name="blogID">blogID in string format</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <returns></returns>
        internal List<MWAPage> GetPages(string blogID, string userName, string password)
        {
            ValidateRequest(userName, password);

            var pages = Ioc.Resolve<IRepository>()
                .Find<Page>()
                .OrderByDescending(x => x.Id);

            var mwaPages = new List<MWAPage>();

            foreach (Page page in pages)
            {
                var mwaPage = new MWAPage();

                mwaPage.pageID = page.Id.ToString();
                mwaPage.title = page.Title;
                mwaPage.pageDate = page.DateCreated;
                //mwaPage.link = page.AbsoluteLink.AbsoluteUri;

                mwaPages.Add(mwaPage);
            }

            return mwaPages;

            //return Ioc.Resolve<IRepository>()
            //    .Find<Page>()
            //    .OrderByDescending(x => x.Id)
            //    .Select(page => new MWAPage()
            //                        {
            //                            pageID = page.Id.ToString(),
            //                            title = page.Title,
            //                            description = page.Content,
            //                            mt_keywords = page.Keywords,
            //                            pageDate = page.DateCreated,
            //                            link = page.AbsoluteLink.AbsoluteUri,
            //                            mt_convert_breaks = "__default__",
            //                            pageParentID = page.Parent.ToString()
            //                        })
            //    .ToList();
        }

        /// <summary>
        /// wp.getPage
        /// </summary>
        /// <param name="blogID">blogID in string format</param>
        /// <param name="pageID">page guid in string format</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <returns>struct with post details</returns>
        internal MWAPage GetPage(string blogID, string pageID, string userName, string password)
        {
            ValidateRequest(userName, password);

            var sendPage = new MWAPage();

            var repository = Ioc.Resolve<IRepository>();
            var page = repository.Get<Page>(x => x.Id == pageID.ToInt());

            sendPage.pageID = page.Id.ToString();
            sendPage.title = page.Title;
            sendPage.description = page.Content;
            sendPage.mt_keywords = page.Keywords;
            sendPage.pageDate = page.DateCreated;
            //sendPage.link = page.AbsoluteLink.AbsoluteUri;
            sendPage.mt_convert_breaks = "__default__";

            return sendPage;
        }

        internal bool EditPage(string blogID, string pageID, string userName, string password, MWAPage mPage, bool publish)
        {
            ValidateRequest(userName, password);

            var repository = Ioc.Resolve<IRepository>();

            var page = repository.Get<Page>(pageID.ToInt());

            page.Title = mPage.title;
            page.Content = mPage.description;
            page.Keywords = mPage.mt_keywords;
            page.ShowInList = publish;
            page.IsPublished = publish;

            repository.Save(page);

            return true;
        }

        internal bool DeletePage(string blogID, string pageID, string userName, string password)
        {
            ValidateRequest(userName, password);
            try
            {
                var repository = Ioc.Resolve<IRepository>();
                var page = repository.Get<Page>(x => x.Id == pageID.ToInt());
                repository.Delete(page);
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("15", "DeletePage failed.  Error: " + ex.Message);
            }

            return true;
        }

        internal List<MWAAuthor> GetAuthors(string blogID, string userName, string password)
        {
            ValidateRequest(userName, password);

            var authors = new List<MWAAuthor>();

            //if (Roles.IsUserInRole(userName, BlogSettings.Instance.AdministratorRole))
            //TODO: Adjust to actual admin role name
            if (Roles.IsUserInRole("Admin"))
            {
                int total = 0;
                int count = 0;
                MembershipUserCollection users = Membership.Provider.GetAllUsers(0, 999, out total);

                foreach (MembershipUser user in users)
                {
                    count++;
                    MWAAuthor author = new MWAAuthor();
                    author.user_id = user.UserName;
                    author.user_login = user.UserName;
                    author.display_name = user.UserName;
                    author.user_email = user.Email;
                    author.meta_value = "";
                    authors.Add(author);
                }
            }
            else
            {
                // If not admin, just add that user to the options.
                MembershipUser single = Membership.GetUser(userName);
                MWAAuthor author = new MWAAuthor();
                author.user_id = single.UserName;
                author.user_login = single.UserName;
                author.display_name = single.UserName;
                author.user_email = single.Email;
                author.meta_value = "";
                authors.Add(author);
            }
            return authors;
        }

        private static void ValidateRequest(string userName, string password)
        {
            if (!Membership.ValidateUser(userName, password))
                throw new MetaWeblogException("11", "User authentication failed");
        }
    }

    [Serializable]
    public class MetaWeblogException : Exception
    {
        public MetaWeblogException(string code, string message)
            : base(message)
        {
            _code = code;
        }

        private string _code;

        public string Code
        {
            get { return _code; }
        }
    }

    internal struct MWACategory
    {
        public string description;
        public string htmlUrl;
        public string rssUrl;
        public string id;
        public string title;
    }

    internal struct MWABlogInfo
    {
        public string url;
        public string blogID;
        public string blogName;
    }

    internal struct MWAFault
    {
        public string faultCode;
        public string faultString;
    }

    internal struct MWAMediaObject
    {
        public string name;
        public string type;
        public byte[] bits;
    }

    internal struct MWAMediaInfo
    {
        public string url;
    }

    internal struct MWAPost
    {
        public string postID;
        public string title;
        public string link;
        public string description;
        public List<string> categories;
        public List<string> tags;
        public DateTime postDate;
        public bool publish;
        public string slug;
        public string commentPolicy;
        public string excerpt;
        public string author;
    }

    internal struct MWAPage
    {
        public string pageID;
        public string title;
        public string link;
        public string description;
        public DateTime pageDate;
        public string mt_convert_breaks;
        public string pageParentID;
        public string mt_keywords;
    }

    internal struct MWAAuthor
    {
        public string user_id;
        public string user_login;
        public string display_name;
        public string user_email;
        public string meta_value;
    }
}