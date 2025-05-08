﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    public class CasheRepository(IConnectionMultiplexer connectionMultiplexer) : ICashRepository
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
        public async Task<string?> GetAsync(string Key)
        {
            var value = await _database.StringGetAsync(Key);
            return value.IsNullOrEmpty ? value : default;
        }

        public async Task SetAsync(string Key, object value, TimeSpan duration)
        {
            var SerlizerObject = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(Key, SerlizerObject, duration);
        }
    }
}
