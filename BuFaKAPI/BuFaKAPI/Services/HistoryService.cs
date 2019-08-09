namespace BuFaKAPI.Services
{
    using System.Threading.Tasks;
    using BuFaKAPI.Models;
    using Microsoft.EntityFrameworkCore;

    public class HistoryService
    {
        private readonly MyContext _context;

        public HistoryService(MyContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Writes a History Object to the Database
        /// </summary>
        /// <param name="oldvalue">A String of the Old Value inside the data</param>
        /// <param name="responsibleUID">The ID of the User responsible for the Action</param>
        /// <param name="historyType">The Type of the Action - "Edit" or "Delete"</param>
        /// <returns>true if the action is completed</returns>
        /// <returns>false if the action is falted</returns>
        public async Task<bool> WriteHistoryObject(string oldvalue, string responsibleUID, string historyType)
        {
            History history = new History
            {
                OldValue = oldvalue,
                ResponsibleUID = responsibleUID,
                User = this._context.User.FindAsync(responsibleUID).Result,
                HistoryType = historyType
            };

            await this._context.History.AddAsync(history);

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
    }
}
