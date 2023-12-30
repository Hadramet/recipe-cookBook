using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Tests
{
    internal class TestFaker
    {
        private const string _recipeXmlContent = @"<?xml version=""1.0""?>
<ListOfRecipes>
    <RecipeData Name=""TestRecipe"" Type=""Vegan""
        Text=""Preheat the oven to 350°F and line a baking sheet with parchment paper. Toss the butternut squash with a drizzle of olive oil and a few generous pinches of salt and pepper. Roast until golden brown, 20 to 25 minutes.
Make the cashew cream: Blend together the drained raw cashews, fresh water, garlic, lemon juice, 1/2 teaspoon salt and pepper.

Make the filling: In a medium skillet, heat a drizzle of olive oil over medium heat. Add the spinach in increments, along with a pinch of salt, and sauté until all the spinach is incorporated and wilted. Remove from heat and let cool slightly. Squeeze out any excess liquid and chop. In a medium bowl, combine the the spinach with the crumbled tofu, oregano, lemon zest, red pepper flakes, at least 1/4 teaspoon salt, freshly ground black pepper and 1 cup of cashew cream. Season to taste, adding more salt and pepper as desired.
Bring a large pot of salted water to a boil. Add the shells and cook according to the package directions until al dente. Drain.

Assemble the shells. Spread ¼ cup of the reserved cashew cream on the bottom of an 11x7-inch baking dish. Fill each cooked shell with some of the filling and a few cubes of butternut squash, and place into the baking dish. Drizzle a little olive oil over the shells, cover with foil, and bake for 15 minutes, or until heated through. Remove from the oven and serve with the remaining cashew cream."">
        <Ingridients>
            <Ingridient Ingr=""Cups cubed butternut squash"" Col=""1/2"" Ed=""cup"" />
            <Ingridient Ingr=""Olive oil"" Col=""1"" Ed=""teaspoon,"" />
            <Ingridient Ingr=""Jumbo shells"" Col=""16"" Ed=""piece"" />
            <Ingridient Ingr=""Raw cashews"" Col=""1.5"" Ed=""cup"" />
            <Ingridient Ingr=""Garlic"" Col=""1"" Ed=""piece"" />
            <Ingridient Ingr=""Fresh lemon juice"" Col=""3.5"" Ed=""tablespoon"" />
            <Ingridient Ingr=""Sea salt"" Col=""1/2"" Ed=""teaspoon,"" />
            <Ingridient Ingr=""Freshly ground pepper"" Col=""1"" Ed=""piece"" />
            <Ingridient Ingr=""Fresh baby spinach"" Col=""4"" Ed=""cup"" />
            <Ingridient Ingr=""Crumbled fim tofu"" Col=""1"" Ed=""cup"" />
            <Ingridient Ingr=""Dried oregano"" Col=""1"" Ed=""teaspoon,"" />
            <Ingridient Ingr=""Lemon zest"" Col=""1/2"" Ed=""piece"" />
            <Ingridient Ingr=""Red pepper flakes"" Col=""1"" Ed=""pinch"" />
            <Ingridient Ingr=""Cashew cream"" Col=""1"" Ed=""cup"" />
            <Ingridient Ingr=""Pepper"" Col=""1"" Ed=""cup"" />
        </Ingridients>
    </RecipeData>
</ListOfRecipes>";


        public static string CreateRecipeFile()
        {
            var fileName = Path.GetTempFileName();
            fileName = fileName.Replace(".tmp", ".xml");
            File.WriteAllText(fileName, _recipeXmlContent);
            return fileName;
        }
        public static void DeleteRecipeFile(string fileName)
        {
            File.Delete(fileName);
        }



    }
}
