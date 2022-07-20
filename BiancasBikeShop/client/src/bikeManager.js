const apiUrl = '/api/bike';

export const getBikes = () => {
    //add implementation here... 
    return fetch(apiUrl)
        .then((res) => res.json())
};

export const getBikeById = (id) => {
    //add implementation here... 
    return fetch(`${apiUrl}/${id}`)
    .then((res) => res.json())
}

export const getBikesInShopCount = () => {
    //add implementation here... 
    return fetch(`${apiUrl}/count`)
    .then((res) => res.json())
}