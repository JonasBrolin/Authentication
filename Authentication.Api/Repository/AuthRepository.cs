using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Authentication.API.Entity;
using Authentication.API.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Authentication.API.Repository
{
    public class AuthRepository : IDisposable
    {
        private AuthContext databaseContext;

        private UserManager<IdentityUser> userManager;

        public AuthRepository()
        {
            this.databaseContext = new AuthContext();
            this.userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(this.databaseContext));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            var user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            IdentityResult result = await this.userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityResult> DeleteUser(string userName)
        {
            var user = this.userManager.Users.First(f => f.UserName == userName);

            IdentityResult result = await this.userManager.DeleteAsync(user);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await this.userManager.FindAsync(userName, password);

            return user;
        }

        public Client FindClient(string clientId)
        {
            var client = this.databaseContext.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var existingToken = this.databaseContext.RefreshTokens.SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                await this.RemoveRefreshToken(existingToken);
            }

            this.databaseContext.RefreshTokens.Add(token);

            return await this.databaseContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            this.databaseContext.RefreshTokens.Remove(refreshToken);

            return await this.databaseContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await this.databaseContext.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                this.databaseContext.RefreshTokens.Remove(refreshToken);
                return await this.databaseContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await this.databaseContext.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return this.databaseContext.RefreshTokens.ToList();
        }

        public void Dispose()
        {
            this.databaseContext.Dispose();
            this.userManager.Dispose();
        }

        public async Task<bool> AddClient(ClientModel clientModel)
        {
            Client client = new Client()
                                {
                                    Id = clientModel.Id,
                                    Secret = Helper.GetHash(clientModel.Secret),
                                    Name = clientModel.Name,
                                    ApplicationType = clientModel.ApplicationType,
                                    Active = clientModel.Active,
                                    RefreshTokenLifeTime = clientModel.RefreshTokenLifeTime,
                                    AllowedOrigin = clientModel.AllowedOrigin
                                };

            this.databaseContext.Clients.Add(client);

            return await this.databaseContext.SaveChangesAsync() > 0;
        }
    }
}