<%@ Control Language="C#" %>
<%@ Register TagPrefix="sf" Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Web.UI" %>
<%@ Import Namespace="Telerik.Sitefinity.MailchimpConnector" %>

<sf:ResourceLinks id="resourcesLinks" runat="server">
        <sf:EmbeddedResourcePropertySetter Name="Telerik.Sitefinity.Resources.Themes.Light.Images.Loadings.sfLoadingFormBtns.gif"
            Static="true" ControlID="loadingImage" ControlPropertyName="ImageUrl" />
</sf:ResourceLinks>

<div class="sfContentCentered">
    <h1 class="sfBreadCrumb"><asp:Literal ID="titleLabel" Text="<%$ Resources:MailchimpConnectorResources, MailchimpConnectorPageDescription %>" runat="server"/></h1>

    <div class="sfMain sfClearfix">
        <div class="sfContent">
            <div id="configurationForm" class="sfBorderCol sfDisplayNone">
                <h2 class="sfBlack"><asp:Literal ID="subTitleLabel" Text="<%$ Resources:MailchimpConnectorResources, ConnectToMailchimpUsingYourMailchimpCredentials %>" runat="server"/></h2>

                <div class="sfForm sfFormIn">
                    <ol class="sfShortField250">
                        <li>
                            <asp:Label ID="apiKeyLabel" AssociatedControlID="apiKeyTextBox" CssClass="sfTxtLbl" Text="<%$ Resources:MailchimpConnectorResources, MailchimpApiKey %>" runat="server"/>
                            <asp:TextBox ID="apiKeyTextBox" runat="server" CssClass="sfTxt" />
                            <p class="sfExample"><asp:Label ID="mailchimpApiKeyHintLabel" Text="<%$ Resources:MailchimpConnectorResources, MailchimpApiKeyHintLabel %>" runat="server"/></p>
                        </li>
                    </ol>
                    <div id="errorMessageWrapper" runat="server" class="sfError" style="display:none;"></div>
                    <div class="sfMTop30 sfMBottom10">
                        <asp:LinkButton ID="connectButton" runat="server" OnClientClick="return false;" CssClass="sfLinkBtn sfPrimary">
                            <span class="sfLinkBtnIn"><asp:Literal runat="server" ID="connectButtonLabel" Text="<%$ Resources:Labels, Connect %>" /></span>
                        </asp:LinkButton>
                        <div id="loadingView" class="sfLoadingFormBtns sfButtonArea" runat="server" style="display: none;">
                            <sf:SfImage ID="loadingImage" runat="server" AlternateText="<%$Resources:Labels, SavingImgAlt %>" />
                        </div>
                    </div>
                </div>
            </div>

            <div id="connectionForm" class="sfDisplayNone sfBorderCol">
                <p class="sfBiggestTxt"><asp:Literal ID="connectionSubTitleLabel" Text="<%$ Resources:Labels, Connection %>" runat="server"/></p>

                <div id="serverDecore" class="sfConnectedToDecore sfNotConnectedToServerDecore">
                    <span class="sfStation"><asp:Literal ID="sitefinityStationLiteral" Text="<%$ Resources:Labels, Sitefinity %>" runat="server"/></span>
                    <div class="sfArrowBase sfArrowTwoWay">
                        <span></span>
                    </div>
                    <span class="sfDestination"><asp:Literal ID="mailchimpDestinationLabel" Text="<%$ Resources:MailchimpConnectorResources, Mailchimp %>" runat="server"/></span>
                </div>

                <ul class="sfMTop10">
                    <li>
                        <asp:LinkButton ID="disconnectReconnectButton" OnClientClick="return false;" runat="server" CssClass="sfLinkBtn sfChange sfMBottom10">
                            <span id="disconnectReconnectButtonText" class="sfLinkBtnIn"></span>
                        </asp:LinkButton>
                    </li>
                    <li><asp:LinkButton Text="<%$ Resources:Labels, ChangeConnection %>" ID="changeConnectionButton" runat="server" OnClientClick="return false;" CssClass="sfGoto sfSmallerTxt" /></li>
                </ul>
            </div>
        </div>
    </div>
</div>