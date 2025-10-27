using Microsoft.EntityFrameworkCore;
using Trixx.Cartoons.Database;
using Trixx.Cartoons.Database.Models.Pack;
using Trixx.Cartoons.Services.Pack.Models;
using Trixx.Common.Models;
using Trixx.Database;

namespace Trixx.Cartoons.Services.Pack
{
    public class CartoonsPackService(CartoonsDatabaseContext cartoonsDatabaseContext, DatabaseContext databaseContext)
    {
        private readonly CartoonsDatabaseContext _cartoonsDatabaseContext = cartoonsDatabaseContext;
        private readonly DatabaseContext _databaseContext = databaseContext;

        public async Task<IReadOnlyCollection<CartoonsPackListItem>> GetCartoonPacksAsync()
        {
            return await _cartoonsDatabaseContext.CartoonsPacks
                .Select(x => new CartoonsPackListItem
                {
                    Id = x.Id,
                    Label = x.Name,
                })
                .ToListAsync();
        }

        public async Task<SelectItem> GetCartoonPackAsync(int id)
        {
            return await _cartoonsDatabaseContext.CartoonsPacks
                .Where(x => x.Id == id)
                .Select(x => new SelectItem
                {
                    Id = x.Id,
                    Label = x.Name,
                })
                .FirstAsync();
        }

        public async Task<CartoonsPackModel> GetCartoonPackCardAsync(int id)
        {
            var pack = await _cartoonsDatabaseContext.CartoonsPacks
                .Where(x => x.Id == id)
                .Select(x => new CartoonsPackModel
                {
                    Name = x.Name,
                    LabelTypes = x.CartoonsPackLabelTypes
                        .OrderBy(lt => lt.Order)
                        .Select(lt => new CartoonsPackModel.LabelType
                        {
                            Id = lt.Id,
                            Name = lt.Name,
                            Cartoons = lt.PackCartoons.OrderByDescending(x => x.SystemObject.CreateDateTime).Select(c => new CartoonsPackModel.CartoonItem
                            {
                                DictionaryCartoonId = c.DictionaryCartoonId,
                                Name = c.DictionaryCartoon.Name,
                                CartoonType = c.DictionaryCartoon.Type,
                                Year = c.DictionaryCartoon.Year,
                                AlternativeNames = string.Join(" | ", c.DictionaryCartoon.AlternativeNames),
                            }).ToList(),
                            Order = lt.Order,
                        })
                        .ToList(),
                })
                .FirstAsync();

            var labelCartoons = pack.LabelTypes.SelectMany(x => x.Cartoons, (lt, c) => c.DictionaryCartoonId).ToList();
            var notLabelCartoons = await _cartoonsDatabaseContext.DictionaryCartoons
                .Where(x => !labelCartoons.Contains(x.Id))
                .OrderByDescending(x => x.SystemObject.CreateDateTime)
                .Select(x => new CartoonsPackModel.CartoonItem
                {
                    DictionaryCartoonId = x.Id,
                    Name = x.Name,
                    CartoonType = x.Type,
                    Year = x.Year,
                    AlternativeNames = string.Join(" | ", x.AlternativeNames),
                })
                .ToListAsync();

            pack.LabelTypes.Add(new CartoonsPackModel.LabelType
            {
                Id = -1,
                Cartoons = notLabelCartoons,
                Name = "Все",
                Order = -1,
            });

            pack.LabelTypes = [.. pack.LabelTypes.OrderBy(x => x.Order)];

            return pack;
        }

        public async Task EditCartoonPackAsync(CartoonsPackEditModel model, int userId)
        {
            var user = await _databaseContext.Users
                .Where(x => x.Id == userId)
                .Select(x => x.FullName)
                .FirstAsync();

            CartoonsPack pack;
            if (model.Id == null)
            {
                pack = new CartoonsPack
                {
                    SystemObject = new Database.Models.CartoonSystemObject
                    {
                        CreateDateTime = DateTime.Now,
                        Type = Database.Enums.SystemObjectType.CartoonPack,
                        Creator = user
                    },
                    CartoonsPackLabelTypes = [],
                };
                _cartoonsDatabaseContext.CartoonsPacks.Add(pack);
            }
            else
            {
                pack = await _cartoonsDatabaseContext.CartoonsPacks
                    .Include(x => x.SystemObject)
                    .Include(x => x.CartoonsPackLabelTypes)
                    .FirstAsync(x => x.Id == model.Id);
            }

            pack.Name = model.Name;

            await _cartoonsDatabaseContext.SaveChangesAsync();
        }

        public async Task EditLabelTypeAsync(CartoonsPackLabelTypeEditModel model, int userId)
        {
            var user = await _databaseContext.Users
                .Where(x => x.Id == userId)
                .Select(x => x.FullName)
                .FirstAsync();

            CartoonsPackLabelType labelType;
            if (model.Id == null)
            {
                labelType = new CartoonsPackLabelType
                {
                    SystemObject = new Database.Models.CartoonSystemObject
                    {
                        CreateDateTime = DateTime.Now,
                        Type = Database.Enums.SystemObjectType.CartoonPack,
                        Creator = user
                    },
                    CartoonsPackId = model.CartoonPackId,
                };
                _cartoonsDatabaseContext.CartoonsPackLabelTypes.Add(labelType);
            }
            else
            {
                labelType = await _cartoonsDatabaseContext.CartoonsPackLabelTypes
                    .Include(x => x.SystemObject)
                    .Include(x => x.CartoonsPack)
                    .FirstAsync(x => x.Id == model.Id);
            }

            labelType.Name = model.Name;
            labelType.Order = model.Order;

            await _cartoonsDatabaseContext.SaveChangesAsync();
        }

        public async Task DeleteLabelTypeAsync(int id)
        {
            var labelType = await _cartoonsDatabaseContext.CartoonsPackLabelTypes
                .FirstAsync(x => x.Id == id);
            _cartoonsDatabaseContext.CartoonsPackLabelTypes
                .Remove(labelType);
            await _cartoonsDatabaseContext.SaveChangesAsync();
        }

        public async Task ReplaceCartoon(int packId, int labelTypeId, int dictionaryCartoonId, int userId)
        {
            if (labelTypeId == -1)
            {
                var cartoon = await _cartoonsDatabaseContext.PackCartoons
                    .FirstOrDefaultAsync(x => x.DictionaryCartoonId == dictionaryCartoonId && x.CartoonsPackLabelType.CartoonsPackId == packId);
                _cartoonsDatabaseContext.PackCartoons.Remove(cartoon);
            }
            else
            {
                var user = await _databaseContext.Users
                    .Where(x => x.Id == userId)
                    .Select(x => x.FullName)
                    .FirstAsync();

                var cartoon = await _cartoonsDatabaseContext.PackCartoons
                        .Include(x => x.SystemObject)
                        .FirstOrDefaultAsync(x => x.DictionaryCartoonId == dictionaryCartoonId && x.CartoonsPackLabelType.CartoonsPackId == packId);

                if (cartoon == null)
                {
                    cartoon = new PackCartoon
                    {
                        SystemObject = new Database.Models.CartoonSystemObject
                        {
                            CreateDateTime = DateTime.Now,
                            Type = Database.Enums.SystemObjectType.CartoonPack,
                            Creator = user
                        },
                        DictionaryCartoonId = dictionaryCartoonId,
                    };
                    _cartoonsDatabaseContext.PackCartoons.Add(cartoon);
                }

                cartoon.CartoonsPackLabelTypeId = labelTypeId;
            }

            await _cartoonsDatabaseContext.SaveChangesAsync();
        }
    }
}
