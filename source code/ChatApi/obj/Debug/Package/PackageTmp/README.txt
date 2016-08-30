This is my implementation of the Chat API.  Please note the following points when attempting to consume:

1. All objects (users, chats, messages) are in-memory. When you start the project, you can call GET /api/loaddata to insert some of the test data from the example.
1a. Due to everything being in memory, I placed all endpoints in a single controller class. If I were connecting to a database, I would break these out accordingly.
2. I ended up using JsonWebTokens instead of OAuth Bearer tokens for authorization.I was simply just not comfortable with OAuth yet to get that working. 
3. Pagination is not available in this version, simply due to time constraints. With more time, I think I would implement it by returning a list of "page" objects, with each page containing a seperate list of chats or messages.