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

 public class RolController : BaseApiController
{

     private readonly IUnitOfWork _unitofwork;
     private readonly IMapper _mapper;

    public RolController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitofwork = unitOfWork;
        _mapper = mapper;
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RolDto>>> Get()
    {
        var Con = await  _unitofwork.Rols.GetAllAsync();
        return _mapper.Map<List<RolDto>>(Con);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Rol>> Post(RolDto rolDto){
        var rol = _mapper.Map<Rol>(rolDto);
        this._unitofwork.Rols.Add(rol);
        await _unitofwork.SaveAsync();
        if(rol == null)
        {
            return BadRequest();
        }
        rolDto.Id = rol.Id.ToString();
        return CreatedAtAction(nameof(Post),new {id= rolDto.Id}, rolDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Rol>> Put(int id, [FromBody]Rol rol){
        if(rol == null)
            return NotFound();
        _unitofwork.Rols.Update(rol);
        await _unitofwork.SaveAsync();
        return rol;
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id){
        var D = await _unitofwork.Rols.GetByIdAsync(id);
        if(D == null){
            return NotFound();
        }
        _unitofwork.Rols.Remove(D);
        await _unitofwork.SaveAsync();
        return NoContent();
    }
}
