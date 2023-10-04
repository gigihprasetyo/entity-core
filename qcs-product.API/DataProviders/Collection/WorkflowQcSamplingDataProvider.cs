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

namespace qcs_product.API.DataProviders.Collection
{
    public class WorkflowQcSamplingDataProvider : IWorkflowQcSamplingDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly IOptions<WorkflowServiceSetting> _WorkflowServiceSetting;

        [ExcludeFromCodeCoverage]
        public WorkflowQcSamplingDataProvider(QcsProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<WorkflowQcSampling> GetByWorkflowByQcSamplingIdIsInWorkflow(int qcSamplingId)
        {
            return await _context.WorkflowQcSampling.FirstOrDefaultAsync(x => x.QcSamplingId == qcSamplingId && x.IsInWorkflow == true);
        }

        public async Task<int> CountWorkflowNotCompleteBySamplingIdPhase2(int qcSamplingId)
        {
            var data = await _context.WorkflowQcSampling.Where(x => x.QcSamplingId == qcSamplingId && x.IsInWorkflow == true && x.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2).ToListAsync();
            return data.Count();
        }

        public async Task<WorkflowQcSampling> GetByWorkflowByQcSamplingIdLatest(int qcSamplingId)
        {
            return await _context.WorkflowQcSampling.Where(x => x.QcSamplingId == qcSamplingId).OrderByDescending(x => x.UpdatedAt).FirstOrDefaultAsync();
        }

        public Task UpdateWorkflowQcSamplingDataFromApproval(UpdateWorkflowQcSamplingFromApproval data)
        {
            var course = _context.WorkflowQcSampling.FirstOrDefault(x => x.QcSamplingId == data.SamplingId && x.IsInWorkflow == true);

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
                    else if ((course.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_QA_NAME) && (data.RowStatus == ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME) || (data.RowStatus == ApplicationConstant.WORKFLOW_ACTION_EDIT_NAME))
                    {
                        course.RowStatus = course.RowStatus;
                    }
                    else if ((course.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_KABAG_NAME) && (data.RowStatus == ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME) || (data.RowStatus == ApplicationConstant.WORKFLOW_ACTION_EDIT_NAME))
                    {
                        course.RowStatus = course.RowStatus;
                    }
                    else
                    {
                        course.RowStatus = data.RowStatus;
                    }
                }
                course.WorkflowStatus = data.WorkflowStatus;
                course.IsInWorkflow = data.IsInWorkflow;
                course.UpdatedBy = data.UpdatedBy;
                course.UpdatedAt = DateTime.UtcNow.AddHours(7);

            }

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task UpdateWorkflowQcSamplingDataFromApprovalNextPhase(UpdateWorkflowQcSamplingFromApproval data)
        {
            var course = _context.WorkflowQcSampling.FirstOrDefault(x => x.QcSamplingId == data.SamplingId && x.IsInWorkflow == true && x.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2);

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

                course.WorkflowStatus = data.WorkflowStatus;
                course.IsInWorkflow = data.IsInWorkflow;
                course.UpdatedBy = data.UpdatedBy;
                course.UpdatedAt = DateTime.UtcNow.AddHours(7);
            }

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task Insert(WorkflowQcSampling data)
        {
            await _context.WorkflowQcSampling.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WorkflowQcSampling>> GetByWorkflowByQcSamplingId(int qcSamplingId)
        {
            return await _context.WorkflowQcSampling.Where(x => x.QcSamplingId == qcSamplingId).ToListAsync();
        }

        public async Task<WorkflowQcSampling> GetByWorkflowByQcSamplingIdAlt(int qcSamplingId)
        {
            return await _context.WorkflowQcSampling.FirstOrDefaultAsync(x => x.QcSamplingId == qcSamplingId);
        }

        public async Task<List<WorkflowQcSampling>> GetByWorkflowPhase2ByQcSamplingId(int qcSamplingId)
        {
            var result = await (from wfs in _context.WorkflowQcSampling
                                where wfs.QcSamplingId == qcSamplingId
                                && wfs.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2
                                && wfs.IsInWorkflow == true
                                select new WorkflowQcSampling
                                {
                                    Id = wfs.Id,
                                    QcSamplingId = wfs.QcSamplingId,
                                    WorkflowCode = wfs.WorkflowCode,
                                    WorkflowDocumentCode = wfs.WorkflowDocumentCode,
                                    IsInWorkflow = wfs.IsInWorkflow,
                                    WorkflowStatus = wfs.WorkflowStatus,
                                    RowStatus = wfs.RowStatus,
                                    CreatedAt = wfs.CreatedAt,
                                    CreatedBy = wfs.CreatedBy,
                                    UpdatedAt = wfs.UpdatedAt,
                                    UpdatedBy = wfs.UpdatedBy,
                                }).OrderByDescending(x => x.UpdatedBy).ToListAsync();

            return result;
        }

        public async Task<WorkflowQcSampling> GetWorkflowByRequestId(int requestId)
        {
            var result = await (from wfs in _context.WorkflowQcSampling
                                join qs in _context.QcSamplings on wfs.QcSamplingId equals qs.Id
                                join qr in _context.RequestQcs on qs.RequestQcsId equals qr.Id
                                where qr.Id == requestId && wfs.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_1
                                select wfs).FirstOrDefaultAsync();

            return result;
        }
        
        public async Task<List<WorkflowQcSampling>> GetInWorkflowBySamplingIds(List<int> samplingIds)
        {
            return await _context.WorkflowQcSampling.Where(x => samplingIds.Contains(x.QcSamplingId) && x.IsInWorkflow == true).ToListAsync();
        }

    }
}
