using ComputerSeekho.API.DTOs.Request;
using ComputerSeekho.API.DTOs.Response;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.API.Services.Interfaces;

namespace ComputerSeekho.API.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IEnquiryRepository _enquiryRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStaffRepository _staffRepository;

        public EnquiryService(
            IEnquiryRepository enquiryRepository,
            ICourseRepository courseRepository,
            IStaffRepository staffRepository)
        {
            _enquiryRepository = enquiryRepository;
            _courseRepository = courseRepository;
            _staffRepository = staffRepository;
        }

        public async Task<Enquiry> CreateEnquiryAsync(EnquiryCreateRequestDTO dto)
        {
            var enquiry = new Enquiry
            {
                EnquirerName = dto.EnquirerName,
                StudentName = dto.StudentName,
                EnquirerAddress = dto.EnquirerAddress,
                EnquirerMobile = dto.EnquirerMobile,
                EnquirerAlternateMobile = dto.EnquirerAlternateMobile,
                EnquirerEmailId = dto.EnquirerEmailId,
                EnquirerQuery = dto.EnquirerQuery,
                EnquirySource = dto.EnquirySource,
                SpecialInstructions = dto.SpecialInstructions
            };

            // Set Course if provided
            if (dto.CourseId.HasValue)
            {
                var course = await _courseRepository.GetByIdAsync(dto.CourseId.Value);
                if (course == null)
                {
                    throw new Exception($"Course not found with id: {dto.CourseId.Value}");
                }
                enquiry.CourseId = dto.CourseId.Value;
            }

            // Set Staff if provided
            if (dto.StaffId.HasValue)
            {
                var staff = await _staffRepository.GetByIdAsync(dto.StaffId.Value);
                if (staff == null)
                {
                    throw new Exception($"Staff not found with id: {dto.StaffId.Value}");
                }
                enquiry.StaffId = dto.StaffId.Value;
            }

            // Set follow-up date (default +3 days if not provided)
            enquiry.FollowupDate = dto.FollowupDate ?? DateTime.Now.AddDays(3);

            return await _enquiryRepository.CreateAsync(enquiry);
        }

        public async Task<Enquiry> GetEnquiryByIdAsync(int id)
        {
            var enquiry = await _enquiryRepository.GetByIdAsync(id);
            if (enquiry == null)
            {
                throw new Exception($"Enquiry not found with id: {id}");
            }
            return enquiry;
        }

        public async Task<Enquiry> UpdateEnquiryAsync(int id, EnquiryUpdateRequestDTO dto)
        {
            var enquiry = await _enquiryRepository.GetByIdAsync(id);
            if (enquiry == null)
            {
                throw new Exception($"Enquiry not found with id: {id}");
            }

            // Update basic details if provided
            if (!string.IsNullOrWhiteSpace(dto.EnquirerName))
            {
                enquiry.EnquirerName = dto.EnquirerName;
            }

            if (dto.StudentName != null)
            {
                enquiry.StudentName = dto.StudentName;
            }

            if (dto.EnquirerAddress != null)
            {
                enquiry.EnquirerAddress = dto.EnquirerAddress;
            }

            if (dto.EnquirerMobile.HasValue)
            {
                enquiry.EnquirerMobile = dto.EnquirerMobile.Value;
            }

            if (dto.EnquirerAlternateMobile.HasValue)
            {
                enquiry.EnquirerAlternateMobile = dto.EnquirerAlternateMobile;
            }

            if (dto.EnquirerEmailId != null)
            {
                enquiry.EnquirerEmailId = dto.EnquirerEmailId;
            }

            if (dto.EnquirerQuery != null)
            {
                enquiry.EnquirerQuery = dto.EnquirerQuery;
            }

            if (dto.EnquirySource != null)
            {
                enquiry.EnquirySource = dto.EnquirySource;
            }

            // Update course if changed
            if (dto.CourseId.HasValue)
            {
                var course = await _courseRepository.GetByIdAsync(dto.CourseId.Value);
                if (course == null)
                {
                    throw new Exception($"Course not found with id: {dto.CourseId.Value}");
                }
                enquiry.CourseId = dto.CourseId.Value;
            }

            // Update follow-up date if provided
            if (dto.FollowupDate.HasValue)
            {
                enquiry.FollowupDate = dto.FollowupDate;
            }

            // Update special instructions
            if (dto.SpecialInstructions != null)
            {
                enquiry.SpecialInstructions = dto.SpecialInstructions;
            }

            return await _enquiryRepository.UpdateAsync(enquiry);
        }

        public async Task<List<EnquiryResponseDTO>> GetUpcomingFollowupsForStaffAsync(int staffId)
        {
            var staff = await _staffRepository.GetByIdAsync(staffId);
            if (staff == null)
            {
                throw new Exception($"Staff not found with id: {staffId}");
            }

            var enquiries = await _enquiryRepository.GetUpcomingFollowupsForStaffAsync(staffId, DateTime.Now);

            return enquiries.Select(MapToResponseDTO).ToList();
        }

        public async Task<List<EnquiryResponseDTO>> GetAllPendingFollowupsAsync()
        {
            var enquiries = await _enquiryRepository.GetAllPendingFollowupsAsync(DateTime.Now);

            return enquiries.Select(MapToResponseDTO).ToList();
        }

        public async Task<Enquiry> UpdateFollowupAsync(EnquiryFollowUpRequestDTO dto)
        {
            var enquiry = await _enquiryRepository.GetByIdAsync(dto.EnquiryId);
            if (enquiry == null)
            {
                throw new Exception($"Enquiry not found with id: {dto.EnquiryId}");
            }

            // Update remarks and special instructions
            if (!string.IsNullOrWhiteSpace(dto.SpecialInstructions))
            {
                enquiry.SpecialInstructions = dto.SpecialInstructions;
            }

            // Increment inquiry counter
            enquiry.InquiryCounter++;

            // Handle closure or next follow-up
            if (dto.CloseEnquiry.HasValue && dto.CloseEnquiry.Value)
            {
                // Close the enquiry
                enquiry.IsClosed = true;

                // Set closure reason if provided
                if (dto.ClosureReasonId.HasValue)
                {
                    enquiry.ClosureReasonId = dto.ClosureReasonId.Value;
                }

                // Set closure reason text (for "Other" option)
                if (!string.IsNullOrWhiteSpace(dto.ClosureReasonText))
                {
                    enquiry.ClosureReasonText = dto.ClosureReasonText;
                }
            }
            else
            {
                // Set next follow-up date
                enquiry.FollowupDate = dto.NextFollowupDate ?? DateTime.Now.AddDays(3);
            }

            return await _enquiryRepository.UpdateAsync(enquiry);
        }

        private EnquiryResponseDTO MapToResponseDTO(Enquiry enquiry)
        {
            return new EnquiryResponseDTO
            {
                EnquiryId = enquiry.EnquiryId,
                EnquirerName = enquiry.EnquirerName,
                EnquirerMobile = enquiry.EnquirerMobile,
                CourseName = enquiry.Course?.CourseName ?? "N/A",
                FollowupDate = enquiry.FollowupDate,
                IsClosed = enquiry.IsClosed
            };
        }
    }
}