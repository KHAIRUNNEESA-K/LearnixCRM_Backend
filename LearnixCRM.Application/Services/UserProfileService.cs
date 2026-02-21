using AutoMapper;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _repository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;

        public UserProfileService(
            IUserProfileRepository repository,
            ICloudinaryService cloudinaryService,
            IMapper mapper)
        {
            _repository = repository;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;

        }
        public async Task<UserProfileDto> GetProfileAsync(int userId)
        {
            var user = await _repository.GetProfileByUserIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException("Profile not found.");

            if (user.Status == UserStatus.Blocked)
                throw new InvalidOperationException("Blocked users cannot access profile.");

            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task<UserProfileDto> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            var user = await _repository.GetProfileByUserIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found.");

            if (user.Status != UserStatus.Active)
                throw new InvalidOperationException("Only active users can update profile.");

            if (!string.IsNullOrWhiteSpace(dto.ContactNumber) &&
                dto.ContactNumber.Length < 10)
            {
                throw new ArgumentException("Invalid contact number.");
            }

            string? imageUrl = user.ProfileImageUrl;
            string? publicId = user.ProfileImagePublicId;


            if (dto.ProfileImage != null)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png" };

                if (!allowedTypes.Contains(dto.ProfileImage.ContentType))
                    throw new InvalidOperationException("Only JPG and PNG images are allowed.");


                var uploadResult = await _cloudinaryService.UploadImageAsync(dto.ProfileImage);

                if (!string.IsNullOrEmpty(user.ProfileImagePublicId))
                {
                    await _cloudinaryService.DeleteImageAsync(user.ProfileImagePublicId);
                }

                imageUrl = uploadResult.Url;
                publicId = uploadResult.PublicId;
            }

            var fullName = $"{dto.FirstName?.Trim()} {dto.LastName?.Trim()}".Trim();

            user.UpdateProfile(
                fullName,
                dto.ContactNumber,
                imageUrl,
                publicId,
                userId
            );

            await _repository.UpdateProfileAsync(user);

            return _mapper.Map<UserProfileDto>(user);
        }


    }
}
