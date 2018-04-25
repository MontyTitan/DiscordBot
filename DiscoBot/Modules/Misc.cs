using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscoBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        private CommandService _service;

        public static bool adminChannel;

        public Misc(CommandService service)
        {
            _service = service;
        }

        [Command("sm")]
        [Summary("Send a message to @user list.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SendUserMessage(SocketUser user, [Remainder] string quote = "")
        {
            string channelID = Context.Channel.Id.ToString();
            adminChannel = CheckChannel(channelID);

            var admin = Context.User as SocketGuildUser;
            var role = (admin as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == Program.roleName);

            if (adminChannel && admin.Roles.Contains(role))
            {
                user = Context.Message.MentionedUsers.FirstOrDefault();
                Utilities.ValidateList(user.Id.ToString(), quote);
                if (Utilities.set)
                    await Context.Channel.SendMessageAsync($"Message for {user.Username} added.");
                else
                    await Context.Channel.SendMessageAsync($":x: Message for {user.Username} could not be added.");
            }
            else
            {
                await Context.Message.DeleteAsync();
                var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync("Please use the admin channel. Thank you.");
            }
        }

        [Command("gm")]
        [Summary("Get random message from @user list.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task GetUserMessage(SocketUser user)
        {
            string channelID = Context.Channel.Id.ToString();
            adminChannel = CheckChannel(channelID);

            var admin = Context.User as SocketGuildUser;
            var role = (admin as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == Program.roleName);

            if (adminChannel && admin.Roles.Contains(role))
            {
                user = Context.Message.MentionedUsers.FirstOrDefault();
                string quote = Utilities.ReturnRandomString(user.Id.ToString());

                if (!Utilities.set)
                {
                    await Context.Channel.SendMessageAsync($"{user.Username} does not have any messages saved.");
                }
                else if (Utilities.set)
                {
                    var embed = new EmbedBuilder();
                    embed.WithTitle("Quote by: " + user.Username);
                    embed.WithDescription(quote);
                    embed.WithColor(new Color(255, 255, 0));

                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
                else
                {
                    //await Context.Channel.SendMessageAsync("No idea what happened");
                }
            }
            else
            {
                await Context.Message.DeleteAsync();
                var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync("Please use the admin channel. Thank you.");
            }
        }

        [Command("lm")]
        [Summary("List all messages from @user list.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ListUserMessage(SocketUser user)
        {
            string channelID = Context.Channel.Id.ToString();
            adminChannel = CheckChannel(channelID);

            var admin = Context.User as SocketGuildUser;
            var role = (admin as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == Program.roleName);

            if (adminChannel && admin.Roles.Contains(role))
            {
                user = Context.Message.MentionedUsers.FirstOrDefault();
                var quotes = Utilities.ReturnStringList(user.Id.ToString());
                int x = 0;

                if (!Utilities.set)
                {
                    await Context.Channel.SendMessageAsync($"{user.Username} does not have any messages saved.");
                }
                else if (Utilities.set)
                {
                    var embedStart = new EmbedBuilder();
                    var embed = new EmbedBuilder();

                    embed.WithTitle("Quotes by: " + user.Username);
                    embed.WithColor(new Color(255, 255, 0));
                    foreach (string entry in quotes)
                    {
                        x++;
                        embed.AddField("Quote" + x, entry);
                    }
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
                else
                {
                    //await Context.Channel.SendMessageAsync("No idea what happened");
                }
            }
            else
            {
                await Context.Message.DeleteAsync();
                var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync("Please use the admin channel. Thank you.");
            }
        }

        [Command("dm")]
        [Summary("Delete specific message from @user list.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DeleteUserMessage(SocketUser user, int a)
        {
            string channelID = Context.Channel.Id.ToString();
            adminChannel = CheckChannel(channelID);

            var admin = Context.User as SocketGuildUser;
            var role = (admin as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == Program.roleName);

            if (adminChannel && admin.Roles.Contains(role))
            {
                user = Context.Message.MentionedUsers.FirstOrDefault();
                Utilities.DeleteString(user.Id.ToString(), a);
                var embed = new EmbedBuilder();

                if (!Utilities.set)
                {
                    embed.WithDescription($"That message does not exist for {user.Username}.");
                    embed.WithColor(new Color(255, 255, 0));
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
                else if (Utilities.set)
                {
                    //var messages = await Context.Channel.GetMessagesAsync(2).Flatten();
                    //await Context.Channel.DeleteMessagesAsync(messages);
                    await Context.Channel.SendMessageAsync($"Message {a} has been deleted for {user.Username}.");
                }
                else
                {
                    //await Context.Channel.SendMessageAsync("No idea what happened");
                }
            }
            else
            {
                await Context.Message.DeleteAsync();
                var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync("Please use the admin channel. Thank you.");
            }
        }

        [Command("du")]
        [Summary("Delete specific user from the list.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DeleteUser(SocketUser user)
        {
            string channelID = Context.Channel.Id.ToString();
            adminChannel = CheckChannel(channelID);

            var admin = Context.User as SocketGuildUser;
            var role = (admin as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == Program.roleName);

            if (adminChannel && admin.Roles.Contains(role))
            {
                user = Context.Message.MentionedUsers.FirstOrDefault();
                Utilities.DeleteUser(user.Id.ToString());
                var embed = new EmbedBuilder();

                if (!Utilities.set)
                {
                    embed.WithDescription($"{user.Username} does not exist in the list.");
                    embed.WithColor(new Color(255, 255, 0));
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
                else if (Utilities.set)
                {
                    //var messages = await Context.Channel.GetMessagesAsync(2).Flatten();
                    //await Context.Channel.DeleteMessagesAsync(messages);
                    await Context.Channel.SendMessageAsync($"All messages for {user.Username} has been deleted.");
                }
                else
                {
                    //await Context.Channel.SendMessageAsync("No idea what happened");
                }
            }
            else
            {
                await Context.Message.DeleteAsync();
                var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync("Please use the admin channel. Thank you.");
            }
        }

        /*[Command("purge")]
        [Summary("Delete number of messages.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task PurgeMessages(int number)
        {
            string channelID = Context.Channel.Id.ToString();
            adminChannel = CheckChannel(channelID);

            if (adminChannel)
            {
                if (number < 100)
                {
                    var messages = await Context.Channel.GetMessagesAsync(number).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages);
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Cannot delete more than 100 messages.");
                }
            }
            else
            {
                await Context.Message.DeleteAsync();
                var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync("Please use the admin channel. Thank you.");
            }
        }*/

        [Command("help")]
        [Summary ("")]
        public async Task HelpAsync(string command = null)
        {
            string channelID = Context.Channel.Id.ToString();
            adminChannel = CheckChannel(channelID);

            var admin = Context.User as SocketGuildUser;
            var role = (admin as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == Program.roleName);

            if (adminChannel && admin.Roles.Contains(role))
            {
                if (command == null)
                {
                    var builder = new EmbedBuilder()
                    {
                        Color = new Color(255, 255, 0),
                        Description = "These are the available commands."
                    };

                    foreach (var module in _service.Modules)
                    {
                        string description = null;
                        foreach (var cmd in module.Commands)
                        {
                            var result = await cmd.CheckPreconditionsAsync(Context);
                            if (result.IsSuccess)
                                description += $"{cmd.Aliases.First()}\n";
                        }

                        if (!string.IsNullOrWhiteSpace(description))
                        {
                            builder.AddField(x =>
                            {
                                x.Name = module.Name;
                                x.Value = description;
                                x.IsInline = false;
                            });
                        }
                    }
                    await ReplyAsync("", false, builder.Build());
                }
                else
                {
                    var result = _service.Search(Context, command);

                    if (!result.IsSuccess)
                    {
                        await ReplyAsync($"Sorry, I couldn't find a command for **{command}**.");
                        return;
                    }

                    var builder = new EmbedBuilder()
                    {
                        Color = new Color(255, 255, 0),
                        Description = $"Help for command **{command}**\n\nAliases: "
                    };

                    foreach (var match in result.Commands)
                    {
                        var cmd = match.Command;

                        builder.AddField(x =>
                        {
                            x.Name = string.Join(", ", cmd.Aliases);
                            x.Value =
                                $"Summary: {cmd.Summary}\n" +
                                $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}: {string.Join("", cmd.Parameters.Select(p => p.Summary))}\n";
                            x.IsInline = false;
                        });
                    }
                    await ReplyAsync("", false, builder.Build());
                }
            }
            else
            {
                await Context.Message.DeleteAsync();
                var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync("Please use the admin channel. Thank you.");
            }
        }

        private static bool CheckChannel(string channelID)
        {

            if (channelID == Program.channelTag)
            {
                return adminChannel = true;
            }
            else
            {
                return !adminChannel;
            }
        }

        /*[Command("myStats")]
        public async Task MyStats([Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            foreach (var user in Context.Message.MentionedUsers)
            {
                //do stuff
            }

            var account = UserAccounts.GetAccount(target);
            await Context.Channel.SendMessageAsync($"{target.Username} has {account.XP} XP and {account.Points} points.");
        }*/

        /*[Command("myStats")]
        public async Task MyStats()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await Context.Channel.SendMessageAsync($"You have {account.XP} XP and {account.Points} points.0");
        }*/

        /*[Command("addXP")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddXP(uint xp)
        {
            var account = UserAccounts.GetAccount(Context.User);
            account.XP += xp;
            UserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync($"You gained {account.XP} XP.");
        }*/

        /*[Command("secret")]
        //[RequireUserPermission(GuildPermission.Administrator)]
        public async Task RevealSecret([Remainder]string arg = "")
        {
            if (!UserIsSecretOwner((SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync(":x: Someone has been naughty and doesn't have the right permissions " + Context.User.Mention);
                return;
            }
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("SECRET"));
        }

        private bool UserIsSecretOwner(SocketGuildUser user)
        {
            string targetRoleName = "SecretOwner";
            var result = from r in user.Guild.Roles
                         where r.Name == targetRoleName
                         select r.Id;
            ulong roleID = result.FirstOrDefault();
            if (roleID == 0) return false;
            var targetRole = user.Guild.GetRole(roleID);
            return user.Roles.Contains(targetRole);
        }*/

        /*[Command("data")]
        //[RequireUserPermission(GuildPermission.Administrator)]
        public async Task GetData()
        {
            await Context.Channel.SendMessageAsync("Data has " + DataStorage.GetPairsCount() + " pairs.");
            //DataStorage.AddPairToStorage("Count" + DataStorage.GetPairsCount(), "TheCount" + DataStorage.GetPairsCount());
            DataStorage.AddPairToStorage("Name: ", Context.User.Username);

            //DataStorage.AddPairToStorage(Context.User.Username, pairs.Add.Context.Message.Content);
        }*/
    }
}
