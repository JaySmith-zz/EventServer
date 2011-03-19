<%@ Page Language="C#" MasterPageFile="Email.Master" Inherits="ViewPage<UserProfile>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="EmailBody" runat="server">

    <h2>Welcome <%= Model.Name %>!</h2>

    <p>
        The username you <%= Html.FullActionLink<AccountController>(c => c.LogOn(""), "log in")%>
        with will always be the email address you have in our system. The password is not retrievable, but you
        can change it or ask us to reset it for you if forgotten.
    </p>

    <p>
        If you plan on presenting, you'll need to first create a <%= Html.FullActionLink<SpeakersController>(c => c.Create(Model.Id), "speaker profile") %>,
        and then <%= Html.FullActionLink<SessionsController>(c => c.Add(), "add sessions") %> to be reviewed.
    </p>

    <p>
        For the sessions you add, you'll notice a comments section at the bottom. These are only visible to you and the administrators and serve
        as our means of communication.
    </p>

    <p>Sincerely,</p>

</asp:Content>
