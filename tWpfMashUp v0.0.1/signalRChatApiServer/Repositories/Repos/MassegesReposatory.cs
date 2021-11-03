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
        public int AddMessage(Message message)//when sending a masssage
        {
            if (message.ChatId <= 0) return -1;
            var id = context.Messages.Add(message).Entity.Id;
            context.SaveChanges();
            return id;
        }
        public IEnumerable<Message> GetMessages(int chatId) //when loading a chat
           => context.Messages.Where(m => m.ChatId == chatId);

        
    }
}
