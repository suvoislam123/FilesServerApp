using AutoMapper;
using Core.Entities;
using System;
using System.IO;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using FileBankAPI.Dto;
using FileBankAPI.Helpers;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FileBankAPI.Errors;

namespace FileBankAPI.Controllers
{
    public class FilesController : BaseApiController
    {
        private readonly IGenericRepository<FileUpload> _filesRepo;
        private readonly IGenericRepository<FileType> _fileTypeRepo;
        private readonly IMapper _mapper;
        private readonly FileBankDbContext _context;
        public FilesController(IGenericRepository<FileUpload> filesRepo,
            IGenericRepository<FileType> fileTypeRepo,
            IMapper mapper,
            FileBankDbContext context
            )
        {
            _filesRepo = filesRepo;
            _fileTypeRepo = fileTypeRepo;
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<FileUploadToReturnDto>>> GetFiles([FromQuery] FileSpecParams specParams)
        {
            var specification = new FilesWithTypeSpecification(specParams);
            var countSpec = new FilesWithFilterForCountSpecification(specParams);

            var totalItems = await _filesRepo.CountAsync(countSpec);

            var products = await _filesRepo.ListAsync(specification);

            var fileDto = _mapper.Map<
            IReadOnlyList<FileUpload>,
            IReadOnlyList<FileUploadToReturnDto>>
            (products);
            return Ok(new Pagination<FileUploadToReturnDto>(specParams.PageIndex, specParams.PageSize, totalItems, fileDto));
        }
        [HttpPost("add")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] FileUploadDto fileUploadDto)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine("wwwroot/Uploads", fileName);
                var fileType = Path.GetExtension(filePath);
                double fileSizeInKilobytes = 0;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                if (filePath != null)
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        long fileSizeInBytes = fileStream.Length;
                        fileSizeInKilobytes = fileSizeInBytes / 1024.0; 
                    }
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                }
                var uploadItem = new FileUpload()
                {
                    Name = fileUploadDto.Name,
                    Description = fileUploadDto.Description,
                    FileUrl = "Uploads/"+fileName,
                    FileSize= fileSizeInKilobytes
                };
                var existFileType = await _context.FileTypes.Where(f => f.TypeName == fileType.ToString()).FirstOrDefaultAsync();
                if(existFileType != null )
                {
                    uploadItem.FileType=existFileType;
                }
                else
                {
                    uploadItem.FileType = new FileType()
                    {
                        TypeName = fileType.ToString()
                    };
                }
                  
            _context.FileUploads.Add(uploadItem);
            await _context.SaveChangesAsync();

                return Ok(new { message = "File uploaded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FileUploadToReturnDto>> GetProduct(int id)
        {
            var specification = new FilesWithTypeSpecification(id);

            var file = await _filesRepo.GetEntityWithSpec(specification);

            if (file == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var fileDto = _mapper.Map<FileUpload, FileUploadToReturnDto>(file);

            return Ok(fileDto);
        }
        [HttpGet("fileTypes")]
        public async Task<ActionResult<IReadOnlyList<FileType>>> GetFiles()
        {
            var types = await _fileTypeRepo.ListAllAsync();

            return Ok(types);
        }
    }
}
