﻿SELECT TABLE_NAME, 
		COLUMN_NAME, 
		COMMENTS 
FROM ALL_COL_COMMENTS  
WHERE OWNER = @OWNER