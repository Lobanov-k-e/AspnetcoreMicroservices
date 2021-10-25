using Dapper;
using Discount.api.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.api.Repositories
{
    public class Repository : IRepository
    {
        private const string SettingKey = "Database:ConnectionString";
        private readonly IConfiguration _configuration;

        public Repository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public async Task<bool> CreateCoupon(Coupon coupon)
        {
            _ = coupon ?? throw new ArgumentNullException(nameof(coupon));

            var connectionString = _configuration.GetValue<string>(SettingKey);
            using var connection = new NpgsqlConnection(connectionString);

            const string Sql = "INSERT INTO COUPON (product_name, description, amount) values (@name, @description, @amount)";
            var values = new { name = coupon.ProductName, description = coupon.Description, amount = coupon.Amount };

            var affectedRows = await connection.ExecuteAsync(Sql, values);
            return affectedRows != 0;
                
        }

        public async Task<bool> DeleteCoupon(string productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                throw new ArgumentException(nameof(productName));
            }

            var connectionString = _configuration.GetValue<string>(SettingKey);
            using var connection = new NpgsqlConnection(connectionString);

            const string Sql = "DELETE FROM Coupon WHERE product_name = @name";
            var values = new { name = productName };
            var affectedRows = await connection.ExecuteAsync(Sql, values);

            return affectedRows != 0;
        }

        public async Task<Coupon> GetCoupon(string productName)
        {
            var connectionString = _configuration.GetValue<string>(SettingKey);
            using var connection = new NpgsqlConnection(connectionString);          

            const string Sql = "SELECT * FROM COUPON WHERE product_name = @ProductName";
            var values = new { ProductName = productName };

            var coupon = await connection
                .QueryFirstOrDefaultAsync<Coupon>(Sql, values);
            return coupon ?? Coupon.NoDiscount(productName);
        }

        public async Task<bool> UpdateCoupon(Coupon coupon)
        {
            _ = coupon ?? throw new ArgumentNullException(nameof(coupon));

            var connectionString = _configuration.GetValue<string>(SettingKey);
            using var connection = new NpgsqlConnection(connectionString);

            const string Sql = "UPDATE COUPON SET product_name=@name, description=@description, amount = @amount WHERE id = @id";
            var values = new 
            {
                id=coupon.Id, 
                name = coupon.ProductName, 
                description = coupon.Description, 
                amount = coupon.Amount 
            };

            var affectedRows = await connection.ExecuteAsync(Sql, values);
            return affectedRows != 0;
        }
    }
}
