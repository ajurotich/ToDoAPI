﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;
using Microsoft.AspNetCore.Cors;

namespace ToDoAPI.Controllers;


[EnableCors]
[Route("api/[controller]")]
[ApiController]
public class ToDosController : ControllerBase {
	private readonly TodoContext _context;

	public ToDosController(TodoContext context) {
		_context = context;
	}

	// GET: api/ToDos
	[HttpGet]
	public async Task<ActionResult<IEnumerable<ToDo>>> GetToDos() {

		var todos = await _context.ToDos
			.Include("Category")
			.Select(
				td => new ToDo {
					Id = td.Id,
					Name = td.Name,
					Description = td.Description,
					Category = new Category() {
						CategoryId = td.Category.CategoryId,
						Name = td.Category.Name,
						Description = td.Category.Description,
					},
					Done = td.Done
				}
			)
			.ToListAsync();

		return Ok(todos);
	}

	// GET: api/ToDos/5
	[HttpGet("{id}")]
	public async Task<ActionResult<ToDo>> GetToDo(int id) {

		var todo = await _context.ToDos
			.Where( td => td.Id == id )
			.Include("Category")
			.Select(
				td => new ToDo {
					Id = td.Id,
					Name = td.Name,
					Description = td.Description,
					Category = new Category() {
						CategoryId = td.Category.CategoryId,
						Name = td.Category.Name,
						Description = td.Category.Description,
					},
					Done = td.Done
				}
			)
			.FirstOrDefaultAsync();

		if(todo == null) 
			return NotFound();

		return todo;
	}

	// PUT: api/ToDos/5
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPut("{id}")]
	public async Task<IActionResult> PutToDo(int id, ToDo toDo) {
		if(id != toDo.Id) {
			return BadRequest();
		}

		_context.Entry(toDo).State = EntityState.Modified;

		try {
			await _context.SaveChangesAsync();
		}
		catch(DbUpdateConcurrencyException) {
			if(!ToDoExists(id)) {
				return NotFound();
			}
			else {
				throw;
			}
		}

		return NoContent();
	}

	// POST: api/ToDos
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPost]
	public async Task<ActionResult<ToDo>> PostToDo(ToDo toDo) {

		ToDo newToDo = new ToDo() {
			Name = toDo.Name,
			Description = toDo.Description,
			CategoryId = toDo.CategoryId,
			Done = false
		};

		_context.ToDos.Add(toDo);
		await _context.SaveChangesAsync();

		return Ok(newToDo);
	}

	// DELETE: api/ToDos/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteToDo(int id) {
		var toDo = await _context.ToDos.FindAsync(id);
		if(toDo == null) {
			return NotFound();
		}

		_context.ToDos.Remove(toDo);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private bool ToDoExists(int id) {
		return _context.ToDos.Any(e => e.Id == id);
	}
}

