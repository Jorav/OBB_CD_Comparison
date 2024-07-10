print (1)
my_data <- read.table("C:/Users/Skuta/Documents/free-time/Programmering/monogame-repos/OBB_CD_Comparison/OBB_CD_Comparison/bin/Debug/netcoreapp3.1/elapsedTimes.txt")
plot(as.numeric(sub(",", ".", my_data[,1], fixed = TRUE)))