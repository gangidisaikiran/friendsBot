using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;

namespace friends
{
    [LuisModel("210ff084-baf0-4c94-a557-f2e6d8466b88", "08b5378f58c94b58b68b6f853d73572b")]
    [Serializable]
    public class JarvisDialog : LuisDialog<object>
    {

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = "Sorry I didn't understand that";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task greeting(IDialogContext context, LuisResult result)
        {
            IMessageActivity reply = context.MakeMessage();            
            reply.Type = "message";
            reply.Attachments = new List<Attachment>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage("https://i.ytimg.com/vi/XSPj7HSYejA/maxresdefault.jpg"));
            List<CardAction> cardActions = new List<CardAction>();
            CardAction sharePizza = new CardAction()
            {
                Title = "Do you want to share your pizza",
                Type = "imBack",
                Value = "Do you want to share your pizza?"
            };
            cardActions.Add(sharePizza);
            HeroCard plCard = new HeroCard()
            {
                Title = "F.R.I.E.N.D.S",
                Subtitle = "Joey Tribbiani",
                Images = cardImages,
                Buttons = cardActions
            };
            reply.Attachments.Add(plCard.ToAttachment());
            await context.PostAsync(reply);
            context.Wait(MessageReceived);
        }

        [LuisIntent("share food")]
        public async Task shareFood(IDialogContext context, LuisResult result)
        {
            IMessageActivity reply = context.MakeMessage();
            reply.Type = "message";
            reply.Attachments = new List<Attachment>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage("http://images-cdn.9gag.com/photo/436051_700b.jpg"));
            List<CardAction> cardActions = new List<CardAction>();
            CardAction help = new CardAction()
            {
                Title = "HELP!",
                Type = "imBack",
                Value = "help"
            };
            cardActions.Add(help);
            HeroCard plCard = new HeroCard()
            {
                Title = "Joey Doesn't Share Food!!",
                Subtitle = "Joey Tribbiani",
                Images = cardImages,
                Buttons = cardActions
            };
            reply.Attachments.Add(plCard.ToAttachment());
            await context.PostAsync(reply);
            context.Wait(MessageReceived);
        }

        public async Task help(IDialogContext context, LuisResult result)
        {
            IMessageActivity reply = context.MakeMessage();
            reply.Type = "message";
            reply.Attachments = new List<Attachment>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage("http://images-cdn.9gag.com/photo/436051_700b.jpg"));
            List<CardAction> cardActions = new List<CardAction>();
            CardAction joey = new CardAction()
            {
                Title = "Joey",
                Type = "imBack",
                Value = "joey"
            };
            CardAction ross = new CardAction()
            {
                Title = "Ross",
                Type = "postBack",
                Value = "ross"
            };
            CardAction chandler = new CardAction()
            {
                Title = "Chandler",
                Type = "postBack",
                Value = "chandler"
            };
            cardActions.Add(ross);
            cardActions.Add(joey);
            cardActions.Add(chandler);
            HeroCard plCard = new HeroCard()
            {
                Title = "F.R.I.E.D.S",
                Subtitle = "We will be there for you...",
                Buttons = cardActions
            };
            reply.Attachments.Add(plCard.ToAttachment());
            await context.PostAsync(reply);
            context.Wait(MessageReceived);

        }


    }

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                    await Conversation.SendAsync(activity, () => new JarvisDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }


        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}