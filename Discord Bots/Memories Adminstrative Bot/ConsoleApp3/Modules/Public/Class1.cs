using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Linq;

namespace ConsoleApp3.Modules.Public
{
    public class Class1 : ModuleBase
    {
        [Command("help")]
        public async Task displayHelp()
        {
            // Hard coded help command
            await ReplyAsync("```" + "Available Commands:\n\nAdminstrative:\n==============\n\naddRole: Adds a role to a user.(Usage: M)addRole USERNAME ROLENAME)\nremRole: Removes a role from a user. (Usage: same as above)\nInfo: Displays useful info on a client. (usage: M)Info USERNAME)\nModeration: moderation commands. (Usage: M)Moderation MODERATION CHANNEL BAN|MUTE|WARN TARGET REASON)\n\n Credits:\n===========\n\nCreator: Nemesis\nLibrary: Discord.NET\nLanguage: C#\nLibrary Version: 1.0.0-rc\nProgram Version: 1.1.3<beta>" + "```");
        }

        [Command("Status")]
        public async Task status()
        {
            await ReplyAsync("```" + "Beep Boop. Billy its working!" + "```");
        }

        [Command("addRole")]
        // [RequireUserPermission(GuildPermissions.Admin)]
        public async Task addGroup(IGuildUser user, [Remainder] string rolename)
        {
            IUser invoker = Context.User;
            var inv = (SocketGuildUser)invoker;
            bool verified = false;
            foreach (var roleVer in inv.Roles)
            {
                if (roleVer.Name == "Bot Admin")
                {
                    verified = true;
                }
            }
            if (verified)
            {
                string currentChannel = Context.Channel.Name;
            //    if (currentChannel == "admin-bot")
            //    {
                    var role = user.Guild.Roles.Where(has => has.Name.ToUpper() == rolename.ToUpper());
                    await user.AddRolesAsync(role);
                    await ReplyAsync("```User " + user.Username + " has been added to the role " + rolename + " ```");
             //   }
            //    else
            //    {
            //        await ReplyAsync("```Unauthorized channel to use this command.```");
            //    }
            } else
            {
                await ReplyAsync("```Unauthorized User```");
            }
        }

        [Command("giveMe")]
        public async Task selfAssign([Remainder] string rolename)
        {
            string[] validRoles = new string[3];
            validRoles[0] = "18+";
            validRoles[1] = "Male";
            validRoles[2] = "Female";

            IUser invoker = Context.User;
            var inv = (SocketGuildUser)invoker;
            bool verified = false;
            foreach (var roleVer in inv.Roles)
            {
                if (roleVer.Name.ToUpper() == "Member".ToUpper() || roleVer.Name.ToUpper() == "Verified".ToUpper())
                {
                    verified = true;
                }
            }
            if (verified)
            {
                bool valid = false;
                string currentChannel = Context.Channel.Name;
                if (currentChannel == "self-assign")
                {
                    for (int i = 0; i < 3; i++) {
                        if (rolename == validRoles[i]) {
                            var role = inv.Guild.Roles.Where(has => has.Name.ToUpper() == rolename.ToUpper());
                            var assign = inv.AddRolesAsync(role);
                            await assign;
                            valid = true;
                            break;
                        }
                    }
                    if (!valid)
                    {
                       await ReplyAsync("```The role you entered is not a valid/authorized role.```");
                    } else
                    {
                        await ReplyAsync("```You should have been assigned the " + rolename + " role if it exists in the server.```");
                    }
                }
                else
                {
                    await ReplyAsync("```Please use the #self-assign text channel to use this command```");
                }
            } else
            {
                await ReplyAsync("```You need to be a member of this server to be able to use this command.```");
            }
        }

        [Command("remRole")]
        public async Task remGroup(IGuildUser user, [Remainder] string rolename)
        {
            IUser invoker = Context.User;
            var inv = (SocketGuildUser)invoker;
            bool verified = false;
            foreach (var roleVer in inv.Roles)
            {
                if (roleVer.Name == "Bot Admin")
                {
                    verified = true;
                }
            }
            string currentChannel = Context.Channel.Name;
            if (verified)
            {
            //    if (currentChannel == "admin-bot")
            //    {
                    var role = user.Guild.Roles.Where(has => has.Name.ToUpper() == rolename.ToUpper());
                    await user.RemoveRolesAsync(role);
                    await ReplyAsync("```User " + user.Username + " has been removed from the role " + rolename + " ```");
            //    }
            //    else
            //    {
            //        await ReplyAsync("```Unauthorized channel.```");
            //    }
            } else
            {
                await ReplyAsync("```Unauthorized User```");
            }
        }

        [Command("info")]
        public async Task info([Remainder] IGuildUser target)
        {
            if (Context.Channel.Name == "admin-bot") {
                int month = target.CreatedAt.Date.Month;
                int day = target.CreatedAt.Date.Day;
                int year = target.CreatedAt.Date.Year;
                int discriminator = target.DiscriminatorValue;
                string avatar = target.AvatarId;
                var status = target.Status;
                
            //    string game = target.Game.Value.Name.ToString();
            //    if (game == null) {
            //        game = "User is currently not in a game";
            //    }
            //    var roles = target.RoleIds;
                ulong clientId = target.Id;
                //   Console.WriteLine(game);
                await ReplyAsync("```User ID: " + clientId + "\nStatus: " + status + "\nCreated on: " + month.ToString() + "/" + day.ToString() + "/" + year.ToString() + "\nUsername: " + target.Username + "\nDiscriminator: " + discriminator + "\nAvatar: " + avatar + "```");
            } else
            {
                await ReplyAsync("```This channel is not authorized for information displayment. Please contact an administrator if you believe this is a mistake.```");
            }
        }
        [Command("moderation")]
        public async Task ban(IGuildChannel channel, string action, IGuildUser target, [Remainder] string Reason)
        {
            IUser invoker = Context.User;
            var inv = (SocketGuildUser)invoker;
            bool staff = false;
            bool verified = true;
            foreach (var roleVer in inv.Roles)
            {
                if (roleVer.Name.ToUpper() == "Moderator".ToUpper())
                {
                    var tar = (SocketGuildUser)target;
                    staff = true;
                    foreach (var roleTar in tar.Roles)
                    {
                        if (roleTar.Name.ToUpper() == "Moderator".ToUpper() || roleTar.Name.ToUpper() == "Developer".ToUpper() || roleTar.Name.ToUpper() == "Dyno".ToUpper() || roleTar.Name.ToUpper() == "Admin".ToUpper() || roleTar.Name.ToUpper() == "Bot Admin".ToUpper() || roleTar.Name.ToUpper() == "Admin Immunity".ToUpper() || roleTar.Name.ToUpper() == "Adminstrative Bots".ToUpper() || roleTar.Name.ToUpper() == "Owner".ToUpper() || roleTar.Name.ToUpper() == "Owner Immunity".ToUpper())
                        {
                            verified = false;
                        }
                    }
                    break;
                }
                else if (roleVer.Name.ToUpper() == "Admin".ToUpper())
                {
                    staff = true;
                    var tar = (SocketGuildUser)target;
                    foreach (var roleTar in tar.Roles)
                    {
                        if (roleTar.Name.ToUpper() == "Dyno".ToUpper() || roleTar.Name.ToUpper() == "Admin".ToUpper() || roleTar.Name.ToUpper() == "Bot Admin".ToUpper() || roleTar.Name.ToUpper() == "Admin Immunity".ToUpper() || roleTar.Name.ToUpper() == "Adminstrative Bots".ToUpper() || roleTar.Name.ToUpper() == "Owner".ToUpper() || roleTar.Name.ToUpper() == "Owner Immunity".ToUpper())
                        {
                            verified = false;
                        }
                    }
                    break;
                }
                else if (roleVer.Name.ToUpper() == "Owner".ToUpper())
                {
                    staff = true;
                    var tar = (SocketGuildUser)target;
                    foreach (var roleTar in tar.Roles)
                    {
                        if (roleTar.Name.ToUpper() == "Dyno".ToUpper() || roleTar.Name.ToUpper() == "Adminstrative Bots".ToUpper() || roleTar.Name.ToUpper() == "Owner".ToUpper() || roleTar.Name.ToUpper() == "Owner Immunity".ToUpper())
                        {
                            verified = false;
                        }
                    }
                    break;
                }
            }
            var modChannel = await Context.Guild.GetChannelAsync(channel.Id) as SocketTextChannel;
			if (verified && staff)
			{
				if (channel.ToString().ToUpper() == "mod-logs".ToUpper())
				{
					if (Context.Channel.Name == "moderation-bot")
					{
						if (action.ToUpper() == "ban".ToUpper())
						{
							await Context.Guild.AddBanAsync(target);
							await modChannel.SendMessageAsync("```Client " + target.Username + " has been banned.\nInvoker: " + invoker.Username + "\nReason: " + Reason + "```");
						}
						else if (action.ToUpper() == "Warn".ToUpper())
						{
							var role = target.Guild.Roles.Where(has => has.Name.ToUpper() == "Warning Issued".ToUpper());
							await target.AddRolesAsync(role);
							await modChannel.SendMessageAsync("```Client " + target.Username + " has been issued a warning.\nInvoker: " + invoker.Username + "\nReason: " + Reason + "```");
						}
						else if (action.ToUpper() == "Mute".ToUpper())
						{
							var role = target.Guild.Roles.Where(has => has.Name.ToUpper() == "text muted".ToUpper());
							await target.AddRolesAsync(role);
							await modChannel.SendMessageAsync("```Client " + target.Username + " has been banned from using text channels.\nInvoker: " + invoker.Username + "\nReason: " + Reason + "```");
						}

					}
					else
					{
						await ReplyAsync("```This channel is not authorized for the use of the moderation bot. Please contact an adminstrator if you believe this is a mistake.```");
					}
				}
				else
				{
					await ReplyAsync("```The moderation channel entered is not authorized. Current authorized channels: 'mod-logs'```");
				}
			}
			else
			{
				await ReplyAsync("```You do not have enough permissions to moderate this client```");
			}
        }
    }
}