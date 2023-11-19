using AutoMapper;
using StudyHub.BLL.Interfaces;
using StudyHub.Common.DTO.Subject;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using System.Data.Entity;

namespace StudyHub.BLL.Services;
public class SubjectService : ISubjectService
{
    private readonly IRepository<Subject> _repository;
    private readonly IMapper _mapper;

    public SubjectService(IRepository<Subject> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SubjectDTO> AddSubject(CreateSubjectDTO dto)
    {
        var entity = _mapper.Map<Subject>(dto);

        if (await _repository.FirstOrDefaultAsync(subject => subject.Title == dto.Title) != null)
            throw new InvalidOperationException("Entity with such key already exists in database");

        await _repository.InsertAsync(entity);
        return _mapper.Map<SubjectDTO>(entity);
    }

    public async Task<bool> DeleteSubject(Guid id)
    {
        var subject = _repository.FirstOrDefault(subject => subject.Id == id);
        return subject != null && await _repository.DeleteAsync(subject);
    }

    public IEnumerable<SubjectDTO> GetSubjects()
    {
        return _mapper.Map<IEnumerable<SubjectDTO>>(_repository.ToList());
    }

    public async Task<SubjectDTO?> GetSubjectsById(Guid id)
    {
        var subject = await _repository.FirstOrDefaultAsync(subject => subject.Id == id);
        return subject != null ? _mapper.Map<SubjectDTO>(subject) : null;
    }

    public async Task<SubjectDTO> UpdateSubject(Guid id, UpdateSubjectDTO dto)
    {
        var subject = _repository.FirstOrDefault(subject => subject.Id == id);

        if (subject == null)
            throw new KeyNotFoundException($"Unable to find entity with such key {id}");

        _mapper.Map(dto, subject);

        await _repository.UpdateAsync(subject);
        return _mapper.Map<SubjectDTO>(subject);
    }
}
