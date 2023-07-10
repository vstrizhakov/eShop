﻿using eShop.Catalog.DbContexts;
using eShop.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.Repositories
{
    public class CompositionRepository : ICompositionRepository
    {
        private readonly CatalogDbContext _context;

        public CompositionRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task CreateCompositionAsync(Composition composition)
        {
            _context.Compositions.Add(composition);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCompositionAsync(Composition composition)
        {
            _context.Compositions.Remove(composition);

            await _context.SaveChangesAsync();
        }

        public async Task<Composition?> GetCompositionByIdAsync(Guid id)
        {
            var composition = await _context.Compositions
                .FindAsync(id);

            return composition;
        }

        public async Task<IEnumerable<Composition>> GetCompositionsAsync(Guid ownerId)
        {
            var compositions = await _context.Compositions
                .Where(e => e.OwnerId == ownerId)
                .ToListAsync();

            return compositions;
        }
    }
}
