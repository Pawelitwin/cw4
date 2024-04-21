using AnimalShelterAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnimalShelterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private static readonly List<Animal> animals = new()
        {
            new Animal { Id = 1, Name = "Max", Category = "dog", Weight = 25, FurColor = "brown" },
            new Animal { Id = 2, Name = "Luna", Category = "cat", Weight = 4, FurColor = "white" }
        };

        private static readonly List<Visit> visits = new()
        {
            new Visit { Id = 1, Date = DateTime.Now, AnimalId = 1, Description = "Regular checkup", Price = 50 },
            new Visit { Id = 2, Date = DateTime.Now.AddDays(-1), AnimalId = 2, Description = "Vaccination", Price = 30 }
        };

        // Pobieranie listy zwierząt
        [HttpGet]
        public IActionResult GetAnimals()
        {
            return Ok(animals);
        }

        
        // Pobieranie konkretnego zwierzęcia po id
        [HttpGet("{id}")]
        public IActionResult GetAnimal(int id)
        {
            var animal = animals.FirstOrDefault(a => a.Id == id);
            if (animal == null)
            {
                return NotFound();
            }
            return Ok(animal);
        }
        
        // Dodawanie nowego zwierzęcia
        [HttpPost]
        public IActionResult AddAnimal(Animal newAnimal)
        {
            newAnimal.Id = animals.Max(a => a.Id) + 1;
            animals.Add(newAnimal);
            return CreatedAtAction(nameof(GetAnimal), new { id = newAnimal.Id }, newAnimal);
        }
        
        // Edycja zwierzęcia
        [HttpPut("{id}")]
        public IActionResult EditAnimal(int id, Animal updatedAnimal)
        {
            var existingAnimal = animals.FirstOrDefault(a => a.Id == id);
            if (existingAnimal == null)
            {
                return NotFound();
            }
            existingAnimal.Name = updatedAnimal.Name;
            existingAnimal.Category = updatedAnimal.Category;
            existingAnimal.Weight = updatedAnimal.Weight;
            existingAnimal.FurColor = updatedAnimal.FurColor;

            return Ok(existingAnimal);
        }
        
        // Usuwanie zwierzęcia
        [HttpDelete("{id}")]
        public IActionResult DeleteAnimal(int id)
        {
            var animal = animals.FirstOrDefault(a => a.Id == id);
            if (animal == null)
            {
                return NotFound();
            }
            animals.Remove(animal);
            return Ok(animals);
        }
        
        // Pobieranie listy wizyt powiązanych z danym zwierzęciem
        [HttpGet("{animalId}/visits")]
        public IActionResult GetVisitsByAnimal(int animalId)
        {
            var animalVisits = visits.Where(v => v.AnimalId == animalId).ToList();
            return Ok(animalVisits);
        }
        
        // Dodawanie nowej wizyty
        [HttpPost("{animalId}/visits")]
        public IActionResult AddVisit(int animalId, Visit newVisit)
        {
            newVisit.Id = visits.Max(v => v.Id) + 1;
            newVisit.AnimalId = animalId;
            visits.Add(newVisit);
            return CreatedAtAction(nameof(GetVisitsByAnimal), new { animalId = animalId }, newVisit);
        }
       
    }

}
