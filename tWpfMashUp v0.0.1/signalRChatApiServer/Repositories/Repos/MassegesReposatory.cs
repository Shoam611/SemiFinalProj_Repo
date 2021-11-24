using signalRChatApiServer.Data;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories.Infra;
using System.Collections.Generic;
using System.Linq;

namespace signalRChatApiServer.Repositories.Repos
{
    public class MassegesReposatory : IMassegesReposatory
    {
        private readonly TalkBackChatContext context;

        public MassegesReposatory(TalkBackChatContext context)
        {
            this.context = context;
        }

        //when sending a masssage
        public bool AddMessage(Message message)
        {
            if (message.ChatId <= 0) return false;
            context.Messages.Add(message);
            context.SaveChanges();
            return true;
        }

        //when loading a chat
        public IEnumerable<Message> GetMessages(int chatId) => context.Messages.Where(m => m.ChatId == chatId);
    }
}
