import numpy as np
from time import time

from api_collector import api_get
from counter import letter_appearance
from char_finder import chars_from, origins_from

# Part 1

# Time GET
start_get = time()

# We get the data from the API
# Print loadings to see progress
print("Getting data locations...")
locations = api_get("location")
print("Getting data characters...")
characters = api_get("character")
print("Getting data episodes...\n")
episodes = api_get("episode")
print("Done!\n")

end_get = time()

# Time count
start_count = time()

# We count the letter appearances
locations_count = letter_appearance("l", locations)
characters_count = letter_appearance("c", characters)
episodes_count = letter_appearance("e", episodes)

# Display results
print(f"The letter \"l\" appears {locations_count} in location names.")
print(f"The letter \"c\" appears {characters_count} in character names.")
print(f"The letter \"e\" appears {episodes_count} in episode names.\n")

end_count = time()

# Part 2

# Time episode locations
start_ep_locs = time()

# Make numpy array from characters list to make faster operations
characters_array = np.array(characters)

# Get characters for each episode applying chars_from on each episode of episodes list
episodes_characters = map(lambda episode: chars_from(episode, characters_array), episodes)

# Get locations sets for each episode applying origins_from on each character set
# These caracter sets were obtained on the previous operation. 
episodes_locations = map(lambda episode_chars: origins_from(episode_chars), episodes_characters)

# Turn the obtained sets into tuples like: (set_lenght, set)
# To obtain how many locations appear on each episode, and their names
episodes_locations_tuples = list(map(lambda episode_locations: (len(episode_locations), episode_locations), episodes_locations))

# Display results
episode_id = 1
for locs in episodes_locations_tuples:
    print(f"{locs[0]} lugares en el episodio {episode_id}: {locs[1]}")
    print()
    episode_id += 1

end_ep_locs = time()

print(f"\n Runtime:")
print(f"   Get:    {end_get - start_get}")
print(f"   Count:  {end_count - start_count}")
print(f"   EpLocs: {end_ep_locs - start_ep_locs}")
print(f"      Total: {end_ep_locs - start_get}")
