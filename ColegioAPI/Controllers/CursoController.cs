using AutoMapper;
using Dominio;
using Dominio.Interfaces;
using Persistencia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ColegioAPI.Dtos;
using ColegioAPI.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace ColegioAPI.Controllers;

[Authorize]

 public class CursoController : BaseApiController
{

     private readonly IUnitOfWork _unitofwork;
     private readonly IMapper _mapper;

    public CursoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitofwork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<CursoDto>>> Get()
    {
        var Con = await  _unitofwork.Courses.GetAllAsync();
        return _mapper.Map<List<CursoDto>>(Con);
    }


  




    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Curso>> Post(CursoDto cursoDto){
        var curso = _mapper.Map<Curso>(cursoDto);
        this._unitofwork.Courses.Add(curso);
        await _unitofwork.SaveAsync();
        if(curso == null)
        {
            return BadRequest();
        }
        cursoDto.Id = curso.Id.ToString();
        return CreatedAtAction(nameof(Post),new {id= cursoDto.Id}, cursoDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Curso>> Put(int id, [FromBody]Curso curso){
        if(curso == null)
            return NotFound();
        _unitofwork.Courses.Update(curso);
        await _unitofwork.SaveAsync();
        return curso;
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id){
        var D = await _unitofwork.Courses.GetByIdAsync(id);
        if(D == null){
            return NotFound();
        }
        _unitofwork.Courses.Remove(D);
        await _unitofwork.SaveAsync();
        return NoContent();
    }
}
