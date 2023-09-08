/* using AutoMapper;
using Dominio;
using Dominio.Interfaces;
using Persistencia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsidenceAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using IncidenceAPI.Helpers;

namespace API.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]
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

    [HttpGet]
    [MapToApiVersion("1.1")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<RolxPersonDto>>> Get11([FromQuery] Params rolParams)
    {
        var Con = await  _unitofwork.Rols.GetAllAsync(rolParams.PageIndex,rolParams.PageSize,rolParams.Search);
        var lstrol = _mapper.Map<List<RolxPersonDto>>(Con.registros);
        return new Pager<RolxPersonDto>(lstrol,Con.totalRegistros,rolParams.PageIndex,rolParams.PageSize,rolParams.Search);
    }

     [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
      public async Task<IActionResult> Get(int id)
    {
        var byidC = await  _unitofwork.Rols.GetByIdAsync(id);
        return Ok(byidC);
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
        rolDto.Id_Rol = rol.Id;
        return CreatedAtAction(nameof(Post),new {id= rolDto.Id_Rol}, rolDto);
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