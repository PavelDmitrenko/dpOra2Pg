﻿SELECT TABLE_NAME, 
		TABLESPACE_NAME, 
		CLUSTER_NAME, 
		IOT_NAME, 
		INI_TRANS, 
		MAX_TRANS, 
		STATUS, 
		BACKED_UP, 
		PCT_INCREASE, 
		MAX_EXTENTS, 
		INITIAL_EXTENT, 
		TEMPORARY, 
		TABLE_TYPE, 
		TABLE_TYPE_OWNER, 
		PARTITIONED, 
		LAST_ANALYZED, 
		SAMPLE_SIZE, 
		TABLE_LOCK, 
		CACHE, 
		INSTANCES, 
		AVG_SPACE_FREELIST_BLOCKS, 
		AVG_ROW_LEN, 
		AVG_SPACE, 
		SEGMENT_CREATED, 
		DURATION, 
		USER_STATS, 
		GLOBAL_STATS, 
		CELL_FLASH_CACHE, 
		FLASH_CACHE, 
		SECONDARY  
FROM ALL_ALL_TABLES  
WHERE OWNER = @OWNER