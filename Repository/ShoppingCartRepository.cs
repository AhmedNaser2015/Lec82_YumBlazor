using Lec82_YumBlazor.Data;
using Lec82_YumBlazor.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Lec82_YumBlazor.Repository
{
    public class ShoppingCartRepository :IShoppingCartRepository
    {
        private  readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> ClearCartAsync(string? userId)
        {
            var cartItems = await _db.ShoppingCart.Where(u => u.UserId == userId).ToListAsync();
            _db.ShoppingCart.RemoveRange(cartItems);
            return await _db.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId)
        {
            return await _db.ShoppingCart.Where(u => u.UserId == userId).Include(u=>u.Product).ToListAsync();
        }
        public async Task<bool> UpdateCartAsync(string userId, int productId, int updateBy)
        {
            if(string.IsNullOrEmpty(userId))
            {               
                return false;
            }

            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == productId);
            if (cart == null)
            {
                cart = new ShoppingCart()
                {
                    UserId = userId,
                    ProductId = productId,
                    Count = updateBy
                };
                await _db.ShoppingCart.AddAsync(cart);
            }
            else 
            {
                cart.Count += updateBy;
                if(cart.Count <= 0)
                {
                    _db.ShoppingCart.Remove(cart);
                }
                //else
                //{
                //    _db.ShoppingCart.Update(cart);
                //}
            }
            //cart.Quantity += updateBy;
            //_db.ShoppingCart.Update(cart);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
