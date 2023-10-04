using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.API.BindingModels;
using qcs_product.API.WorkflowModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using qcs_product.API.SettingModels;
using System.Net.Http;
using System.Net.Http.Headers;
using qcs_product.Constants;
using qcs_product.API.Helpers;

namespace qcs_product.API.DataProviders.Collection
{
    public class WorkflowQcTransactionGroupDataProvider : IWorkflowQcTransactionGroupDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly IOptions<WorkflowServiceSetting> _WorkflowServiceSetting;

        private readonly IQcRequestDataProvider _requestDataProvider;

        [ExcludeFromCodeCoverage]
        public WorkflowQcTransactionGroupDataProvider(
            QcsProductContext context,
            IQcRequestDataProvider requestDataProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _requestDataProvider = requestDataProvider ?? throw new ArgumentNullException(nameof(requestDataProvider));
        }

        public async Task<WorkflowQcTransactionGroup> GetByWorkflowByQcTransactionGroupIdIsInWorkflow(int qcTransactionGroupId)
        {
            return await _context.WorkflowQcTransactionGroup.FirstOrDefaultAsync(x => x.QcTransactionGroupId == qcTransactionGroupId && x.IsInWorkflow == true);
        }

        public async Task<WorkflowQcTransactionGroup> GetByWorkflowByQcTransactionGroupIdLatest(int qcTransactionGroupId)
        {
            return await _context.WorkflowQcTransactionGroup.Where(x => x.QcTransactionGroupId == qcTransactionGroupId).OrderByDescending(x => x.UpdatedAt).FirstOrDefaultAsync();
        }
        public async Task<List<WorkflowQcTransactionGroup>> GetByWorkflowByQcTransactionGroupIdIsInWorkflowAlt(int qcTransactionGroupId)
        {
            return await _context.WorkflowQcTransactionGroup.Where(x => x.QcTransactionGroupId == qcTransactionGroupId).ToListAsync();
        }

        public async Task<WorkflowQcTransactionGroup> UpdateWorkflowQcTransactionGroupDataFromApproval(UpdateWorkflowQcTransactionGroupFromApproval data)
        {
            var course = _context.WorkflowQcTransactionGroup.FirstOrDefault(x => x.QcTransactionGroupId == data.TransactionGroupId && x.IsInWorkflow == true);

            if (course != null)
            {
                if ((course.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA) && (data.RowStatus == ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME))
                {
                    course.RowStatus = ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME;
                }
                else
                {
                    if ((course.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_KABAG_NAME) && (data.RowStatus == ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME) && (course.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG))
                    {
                        course.RowStatus = ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME;
                    }
                    else if ((course.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_QA_NAME) && ((data.RowStatus == ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME) || (data.RowStatus == ApplicationConstant.WORKFLOW_ACTION_EDIT_NAME)))
                    {
                        course.RowStatus = course.RowStatus;
                    }
                    else if ((course.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_KABAG_NAME) && ((data.RowStatus == ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME) || (data.RowStatus == ApplicationConstant.WORKFLOW_ACTION_EDIT_NAME)))
                    {
                        course.RowStatus = course.RowStatus;
                    }
                    else
                    {
                        course.RowStatus = data.RowStatus;
                    }
                }
                course.QcTransactionGroupId = data.TransactionGroupId;
                course.WorkflowStatus = data.WorkflowStatus;
                course.IsInWorkflow = data.IsInWorkflow;
                course.UpdatedBy = data.UpdatedBy;
                course.UpdatedAt = DateTime.UtcNow.AddHours(7);
            }

            //update receiptdate in master data sampling 
            var GetIdSampling = _context.QcTransactionGroups.Join(
                _context.QcTransactionSamples,
                qtg => qtg.Id,
                qts => qts.QcTransactionGroupId,
                (qtg, qts) => new { qtg, qts }
            ).Join(
                _context.QcSamples,
                combineQtg => combineQtg.qts.QcSampleId,
                sample => sample.Id,
                (combineQtg, sample) => new { samplingId = sample.QcSamplingId, qcTransactionGroupId = combineQtg.qtg.Id }
            ).FirstOrDefault(x => x.qcTransactionGroupId == data.TransactionGroupId);

            var sampling = _context.QcSamplings.FirstOrDefault(x => x.Id == GetIdSampling.samplingId);
            if (sampling != null)
            {
                sampling.ReceiptDate = DateHelper.Now();
            }

            //jika status berubah menjadi review QA
            if (data.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA)
            {
                await _requestDataProvider.UpdateReceiptDate(sampling.RequestQcsId, true);
            }
            else if (data.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG)
            {
                await _requestDataProvider.UpdateReceiptDate(sampling.RequestQcsId, false);
            }

            await _context.SaveChangesAsync();

            return course;
        }

        public async Task Insert(WorkflowQcTransactionGroup data)
        {
            await _context.WorkflowQcTransactionGroup.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WorkflowQcTransactionGroup>> GetByWorkflowByQcTransactionGroupId(int qcTransactionGroupId)
        {
            return await _context.WorkflowQcTransactionGroup.Where(x => x.QcTransactionGroupId == qcTransactionGroupId).ToListAsync();
        }
        
        public async Task<List<WorkflowQcTransactionGroup>> GetInWorkflowByTransactionGroupIds(List<int> qcTransactionGroupIds)
        {
            return await _context.WorkflowQcTransactionGroup
                .Where(x => qcTransactionGroupIds.Contains(x.QcTransactionGroupId) && x.IsInWorkflow == true)
                .ToListAsync();
        }

    }
}
