using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.Constants;

namespace qcs_product.API.DataProviders.Collection
{
    public class ItemDataProvider : IItemDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<ItemDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public ItemDataProvider(QcsProductContext context, ILogger<ItemDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<ItemViewModel>> List()
        {
            var result = await (from p in _context.Items
                                where
                                    p.RowStatus == null
                                select new ItemViewModel
                                {
                                    Id = p.Id,
                                    ItemCode = p.ItemCode,
                                    Name = p.Name,
                                    ProductFormId = p.ProductFormId,
                                    ItemGroupId = p.ItemGroupId.ToString(),
                                    ItemGroupName = p.ItemGroupName,
                                    LabelId = p.LabelId.ToString(),
                                    OrgId = p.OrgId,
                                    OrgName = p.OrgName,
                                }).ToListAsync();
            return result;
        }
        
        public async Task<ItemViewModel> GetById(int id)
        {
            var result = await (from p in _context.Items
                                where
                                    p.Id == id &&
                                    p.RowStatus == null
                                select new ItemViewModel
                                {
                                    Id = p.Id,
                                    ItemCode = p.ItemCode,
                                    Name = p.Name,
                                    ProductFormId = p.ProductFormId,
                                    ItemGroupId = p.ItemGroupId.ToString(),
                                    ItemGroupName = p.ItemGroupName,
                                    LabelId = p.LabelId.ToString(),
                                    OrgId = p.OrgId,
                                    OrgName = p.OrgName,
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<ItemRelationViewModel>> GetDetailRelationById(int id)
        {
            var result = await (from p in _context.Items
                                join st in _context.StorageTemperatures on p.Id equals st.ItemId into TempItem
                                from m in TempItem.DefaultIfEmpty()
                                where p.Id == id && p.RowStatus == null
                                && p.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                select new ItemRelationViewModel
                                {
                                    Item = new ItemViewModel
                                    {
                                        Id = p.Id,
                                        ItemCode = p.ItemCode,
                                        Name = p.Name,
                                        ProductFormId = p.ProductFormId,
                                        ItemGroupId = p.ItemGroupId.ToString(),
                                        ItemGroupName = p.ItemGroupName,
                                        LabelId = p.LabelId.ToString(),
                                        OrgId = p.OrgId,
                                        OrgName = p.OrgName,
                                    },
                                    ProductForm = (from idf in _context.ItemDosageForms
                                                   where idf.Id == p.ProductFormId
                                                   select new ProductFormViewModel
                                                   {
                                                       Id = idf.Id,
                                                       FormCode = idf.ItemDosageFormCode,
                                                       Name = idf.ItemDosageFormName
                                                   }).FirstOrDefault(),
                                    ProductGroup = (from ipg in _context.ItemProductGroups
                                                    where ipg.Id == p.ProductGroupId
                                                    select new ProductGroupViewModel
                                                    {
                                                        Id = ipg.Id,
                                                        GroupCode = ipg.ItemProductGroupCode,
                                                        Name = ipg.ItemProductGroupName
                                                    }).FirstOrDefault(),
                                    ProductPresentation = (from ip in _context.ItemPresentations
                                                           join riip in _context.RelItemsItemPresentations on ip.Id equals riip.ItemPresentationId
                                                           where riip.ItemId == p.Id
                                                           select new ProductPresentationViewModel
                                                           {
                                                               Id = ip.Id,
                                                               PresentationCode = ip.ItemPresentationCode,
                                                               PresentationName = ip.ItemPresentationName
                                                           }).FirstOrDefault(),
                                    StorageTemperature = new StorageTemperatureViewModel
                                    {
                                        Id = m.Id,
                                        ItemId = m.ItemId,
                                        Name = m.Name,
                                        TresholdOperator = m.TresholdOperator,
                                        TresholdOperatorName = (from ec in _context.EnumConstant
                                                                where ec.TypeId.ToString() == m.TresholdOperator && ec.KeyGroup == "threshold_operator"
                                                                select ec.Name).FirstOrDefault(),
                                        TresholdValue = m.TresholdValue,
                                        TresholdMin = m.TresholdMin,
                                        TresholdMax = m.TresholdMax
                                    },
                                    ProductTestTypes = (from ptt in _context.ProductTestTypes
                                                        join tt in _context.TestTypes on ptt.TestTypeId equals tt.Id
                                                        join o in _context.Organizations on tt.OrgId equals o.Id
                                                        where ptt.ItemId == p.Id
                                                        select new ProductTestTypeViewModel
                                                        {
                                                            Id = ptt.Id,
                                                            ItemId = p.Id,
                                                            TestTypeId = tt.Id,
                                                            Name = tt.Name,
                                                            OrgId = tt.OrgId,
                                                            OrgName = o.Name,
                                                            SampleAmountCount = ptt.SampleAmountCount,
                                                            SampleAmountVolume = ptt.SampleAmountVolume,
                                                            SampleAmountUnit = ptt.SampleAmountUnit,
                                                            SampleAmountPresentation = ptt.SampleAmountPresentation,
                                                            RowStatus = ptt.RowStatus
                                                        }).ToList(),
                                    ProductProductionPhase = (from ppp in _context.ProductProductionPhases
                                                              where ppp.ItemId == p.Id
                                                              select
                                                        new ProductProductionPhaseViewModel
                                                        {
                                                            Id = ppp.Id,
                                                            ProductProdPhaseCode = ppp.ProductProdPhaseCode,
                                                            ItemId = p.Id,
                                                            ItemName = p.Name,
                                                            Name = ppp.Name,
                                                            Rooms = (from r in _context.Rooms
                                                                     join rpppr in _context.RelProductProdPhaseToRooms on r.Id equals rpppr.RoomId
                                                                     where rpppr.ProductProductionPhasesId == ppp.Id
                                                                     select new RoomViewModel
                                                                     {
                                                                         Id = r.Id,
                                                                         Code = r.Code,
                                                                         Name = r.Name
                                                                     }).ToList(),
                                                        }).ToList(),
                                    BatchNumbers = (from b in _context.ItemBatchNumbers
                                                    where b.ItemId == p.Id
                                                    select new ItemBatchNumberViewModel
                                                    {
                                                        Id = b.Id,
                                                        BatchNumber = b.BatchNumber,
                                                        ExpireDate = b.ExpireDate
                                                    }).ToList()

                                }).ToListAsync();

            _logger.LogInformation($"Data result : {result}");


            return result;
        }

        public async Task<List<ItemBatchRelationViewModel>> ListItemBatch(string search, int GroupId, List<string> groupCode, DateTime? startDate, DateTime? endDate)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from p in _context.Items
                                join g in _context.ItemGroups on p.ItemGroupId equals g.Id
                                where (EF.Functions.Like(p.Name.ToLower(), "%" + filter + "%"))
                                && groupCode.Contains(g.ItemGroupCode)
                                && p.RowStatus == null
                                && p.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                select new ItemBatchRelationViewModel
                                {
                                    Id = p.Id,
                                    ItemCode = p.ItemCode,
                                    Name = p.Name,
                                    ProductFormId = p.ProductFormId,
                                    ItemGroupId = (p.ItemGroupId == 0 ? 0 : p.ItemGroupId),
                                    ItemGroupName = p.ItemGroupName,
                                    LabelId = (p.LabelId == 0 ? 0 : p.LabelId),
                                    OrgId = p.OrgId,
                                    OrgName = p.OrgName,
                                    BatchNumbers = (from b in _context.ItemBatchNumbers
                                                    where b.ItemId == p.Id
                                                    select new ItemBatchNumberViewModel
                                                    {
                                                        Id = b.Id,
                                                        BatchNumber = b.BatchNumber,
                                                        ExpireDate = b.ExpireDate
                                                    }).Where(x => ((x.ExpireDate >= startDate || !startDate.HasValue) && (x.ExpireDate >= endDate || !endDate.HasValue)))
                                                    .OrderByDescending(x => x.ExpireDate).ToList()
                                }).Where(x => x.ItemGroupId == GroupId || GroupId == 0).ToListAsync();

            return result;
        }

        public async Task<List<ItemBatchRelationViewModel>> ListMediaWithBatch(string search, DateTime? startDate, DateTime? endDate)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from p in _context.Items
                                join g in _context.ItemGroups on p.ItemGroupId equals g.Id
                                where (EF.Functions.Like(p.Name.ToLower(), "%" + filter + "%"))
                                && g.ItemGroupCode.ToLower() == ApplicationConstant.MEDIA.ToLower()
                                && p.RowStatus == null
                                && p.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                select new ItemBatchRelationViewModel
                                {
                                    Id = p.Id,
                                    ItemCode = p.ItemCode,
                                    Name = p.Name,
                                    ProductFormId = p.ProductFormId,
                                    ItemGroupId = (p.ItemGroupId == 0 ? 0 : p.ItemGroupId),
                                    ItemGroupName = p.ItemGroupName,
                                    LabelId = (p.LabelId == 0 ? 0 : p.LabelId),
                                    OrgId = p.OrgId,
                                    OrgName = p.OrgName,
                                    BatchNumbers = (from b in _context.ItemBatchNumbers
                                                    where b.ItemId == p.Id
                                                    select new ItemBatchNumberViewModel
                                                    {
                                                        Id = b.Id,
                                                        BatchNumber = b.BatchNumber,
                                                        ExpireDate = b.ExpireDate,
                                                        Quantity = b.Quantity
                                                    }).Where(x => ((x.ExpireDate >= startDate || !startDate.HasValue) && (x.ExpireDate >= endDate || !endDate.HasValue)))
                                                    .OrderByDescending(x => x.ExpireDate).ToList(),
                                }).ToListAsync();

            foreach (var itemBatchRel in result)
            {
                foreach (var itemBatch in itemBatchRel.BatchNumbers)
                {
                    itemBatch.ItemBatchQuotation = await ItemMediaQuotation(itemBatch.BatchNumber);
                }
            }
            
            return result;
        }

        public async Task<ItemSingleBatchRelViewModel> GetItemBatchById(int id)
        {
            var result = await (from i in _context.Items
                                where i.Id == id && i.RowStatus == null
                                select new ItemSingleBatchRelViewModel
                                {
                                    Id = i.Id,
                                    ItemCode = i.ItemCode,
                                    Name = i.Name,
                                    ProductFormId = i.ProductFormId,
                                    ItemGroupId = i.ItemGroupId,
                                    ItemGroupName = i.ItemGroupName,
                                    LabelId = i.LabelId,
                                    OrgId = i.OrgId,
                                    OrgName = i.OrgName,
                                    /*BatchNumbers = (from ib in _context.ItemBatchNumbers
                                                    where ib.ItemId == i.Id && ib.RowStatus == null
                                                    select new ItemBatchNumberViewModel
                                                    {
                                                        Id = ib.Id,
                                                        BatchNumber = ib.BatchNumber,
                                                        ExpireDate = ib.ExpireDate
                                                    }).OrderByDescending(x => x.ExpireDate).FirstOrDefault()*/
                                }).FirstOrDefaultAsync();
            return result;

        }

        public async Task<List<ShortDataListViewModel>> ShortList(string search, int GroupId)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from i in _context.Items
                                join ibn in _context.ItemBatchNumbers on i.Id equals ibn.ItemId
                                where (EF.Functions.Like(i.Name.ToLower(), "%" + filter + "%"))
                                && i.RowStatus == null
                                && i.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                select new ShortDataListViewModel
                                {
                                    Id = i.Id,
                                    Label = i.Name,
                                    Code = ibn.BatchNumber,
                                    GroupId = i.ItemGroupId,
                                    GroupName = i.ItemGroupName
                                }).Where(x => x.GroupId == GroupId || GroupId == 0).OrderBy(x => x.Label).ToListAsync();

            return result;
        }

        public async Task<ItemBatchItemGroupViewModel> ByItemBatch(string itemCode, string batchNumber)
        {
            var result = await (from data in _context.ItemBatchNumbers
                                join item_data in _context.Items
                                    on data.ItemId equals item_data.Id
                                where
                                    item_data.ItemCode == itemCode &&
                                    data.BatchNumber == batchNumber
                                select new ItemBatchItemGroupViewModel
                                {
                                    Id = data.Id,
                                    ItemId = data.ItemId,
                                    ItemCode = item_data.ItemCode,
                                    ItemName = item_data.Name,
                                    BatchNumber = data.BatchNumber,
                                    ExpDate = data.ExpireDate,
                                    CreatedAt = data.CreatedAt,
                                    CreatedBy = data.CreatedBy,
                                    UpdatedAt = data.UpdatedAt,
                                    UpdatedBy = data.UpdatedBy
                                }
                                ).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Item> GetItemByCode(string itemCode)
        {
            var result = await (from data in _context.Items
                                where
                                    data.ItemCode == itemCode
                                select new Item
                                {
                                    Id = data.Id,
                                    ItemCode = data.ItemCode,
                                    Name = data.Name,
                                    CreatedAt = data.CreatedAt,
                                    CreatedBy = data.CreatedBy,
                                    UpdatedAt = data.UpdatedAt,
                                    UpdatedBy = data.UpdatedBy
                                }
                                ).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Item> Insert(Item data)
        {
            await _context.Items.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<Item> Update(Item data)
        {
            _context.Items.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<ItemBatchNumber> InsertBatch(ItemBatchNumber data)
        {
            await _context.ItemBatchNumbers.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<ItemBatchNumber> UpdateBatch(ItemBatchNumber data)
        {
            _context.ItemBatchNumbers.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<ItemGroups> GetItemGroupsByCode(string code)
        {
            return await _context.ItemGroups.FirstOrDefaultAsync(x => x.ItemGroupCode.ToLower() == code.ToLower());
        }
        
        public async Task<ItemBatchQuotationViewModel> ItemMediaQuotation(string batchNumber)
        {
            ItemBatchNumberViewModel itemBatchNumber = await (from itemBatch in _context.ItemBatchNumbers
                where itemBatch.RowStatus == null && itemBatch.BatchNumber == batchNumber

                select new ItemBatchNumberViewModel 
                {
                        Id = itemBatch.Id,
                        BatchNumber = itemBatch.BatchNumber,
                        ExpireDate = itemBatch.ExpireDate,
                        Quantity = itemBatch.Quantity
                }).OrderByDescending(x => x.ExpireDate).FirstOrDefaultAsync();
            
            List<ItemBatchQuotationHistoryViewModel> qcRequestSamplingRelationViewModel = await (from qcSample in _context.QcSamples
                                join qcSamplingMaterial in _context.QcSamplingMaterials on qcSample.QcSamplingMaterialsId equals qcSamplingMaterial.Id
                                where qcSample.RowStatus == null && qcSamplingMaterial.NoBatch == batchNumber
                                
                                select new ItemBatchQuotationHistoryViewModel
                                {
                                    QcSampleId = qcSample.Id,
                                    QcSamplingMaterialsId = qcSamplingMaterial.Id,
                                    SamplingPointId = qcSample.SamplingPointId,
                                    NoBatch = qcSamplingMaterial.NoBatch,
                                }).ToListAsync();

            return new ItemBatchQuotationViewModel()
            {
                CurrentQuantity = (itemBatchNumber.Quantity - qcRequestSamplingRelationViewModel.Count()),
                // QcSampleHistory = qcRequestSamplingRelationViewModel
            };
        }
    }
}
