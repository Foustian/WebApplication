using System;
using IQMedia.Web.Logic;
using IQMedia.Model;
using IQMedia.Web.Logic.Base;
using System.Collections.Generic;

public class PlayerLogic : ILogic
{
    public PlayerObjectsModel GetRawMediaPlayerNCC(PlayerParamModel PlayerParams,string pmgurl)
    {
        PlayerObjectsModel playerObjects = new PlayerObjectsModel();

        if (PlayerParams.EnableCC)
        {
            string highlightContent = string.Empty;
            //if (!string.IsNullOrWhiteSpace(PlayerParams.SearchTerm))
            //{
                int? captionOffset;

                string captionContent = string.Empty;
                List<int> SearchTermList = new List<int>(); // needed for TAds 
                highlightContent = UtilityLogic.GetRawMediaCaption(PlayerParams.SearchTerm, PlayerParams.RawMediaGUID, out captionOffset, out captionContent,pmgurl, out SearchTermList);
                playerObjects.HighlightHtml = highlightContent;
                playerObjects.CCHtml = captionContent;

                if (captionOffset != null && (captionOffset.Value - PlayerParams.PlayerDefaultOffset) >= 0)
                {
                    captionOffset = captionOffset.Value - PlayerParams.PlayerDefaultOffset;
                }
                else
                {
                    captionOffset = 0;
                }

                if (PlayerParams.Offset == null || PlayerParams.Offset==0)
                {
                    PlayerParams.Offset = captionOffset;
                }
            //}
        }

        string rawMediaObject = UtilityLogic.RenderBasicRawMediaPlayer(string.Empty,
                                       Convert.ToString(PlayerParams.RawMediaGUID),
                                       "true",
                                       "false",
                                        Convert.ToString(PlayerParams.ClientGUID),
                                       "false",
                                        Convert.ToString(PlayerParams.CustomerGUID),
                                       PlayerParams.ServiceBaseURL,
                                       PlayerParams.Offset,
                                       PlayerParams.IsActivePlayerLogo,
                                       PlayerParams.PlayerLogoImage,
                                        PlayerParams.BrowserType,
                                        PlayerParams.KeyValues,
                                        PlayerParams.AutoPlayback,
                                        PlayerParams.PreviewImageOption,
                                        PlayerParams.PreviewImageURL);

        playerObjects.VideoHtml = rawMediaObject;

        return playerObjects;
    }
}