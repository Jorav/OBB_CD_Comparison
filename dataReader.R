#read data from file
data_controller_unoptimised_10 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/controller_unoptimised_10.txt")
data_controller_unoptimised_100 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/controller_unoptimised_100.txt")
data_controller_unoptimised_1000 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/controller_unoptimised_1000.txt")
data_controller_SAT_10 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/controller_SAT_10.txt")
data_controller_SAT_100 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/controller_SAT_100.txt")
data_controller_SAT_1000 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/controller_SAT_1000.txt")
data_controller_boundingSpheres_10 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/controller_boundingSpheres_10.txt")
data_controller_boundingSpheres_100 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/controller_boundingSpheres_100.txt")
data_controller_boundingSpheres_1000 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/controller_boundingSpheres_1000.txt")
data_AABB_TD_median_10 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/AABB_TD_median_10.txt")
data_AABB_TD_median_100 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/AABB_TD_median_100.txt")
data_AABB_TD_median_1000 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/AABB_TD_median_1000.txt")
data_AABB_TD_SAH_10 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/AABB_TD_SAH_10.txt")
data_AABB_TD_SAH_100 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/AABB_TD_SAH_100.txt")
data_AABB_TD_SAH_1000 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/AABB_TD_SAH_1000.txt")
data_AABB_insertion_SAH_10 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/AABB_insertion_SAH_10.txt")
data_AABB_insertion_SAH_100 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/AABB_insertion_SAH_100.txt")
data_AABB_insertion_SAH_1000 <- read.table("C:/Users/Skuta/Documents/kandidatarbete/tider/AABB_insertion_SAH_1000.txt")

#convert data to numeric matrix
library(stringr)
basicCD_unoptimised_10 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_unoptimised_10[,1], ";",4), fixed = TRUE))),ncol = 4)
basicCD_unoptimised_100 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_unoptimised_100[,1], ";",4), fixed = TRUE))),ncol = 4)
basicCD_unoptimised_1000 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_unoptimised_1000[,1], ";",4), fixed = TRUE))),ncol = 4)
basicCD_SAT_10 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_SAT_10[,1], ";",4), fixed = TRUE))),ncol = 4)
basicCD_SAT_100 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_SAT_100[,1], ";",4), fixed = TRUE))),ncol = 4)
basicCD_SAT_1000 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_SAT_1000[,1], ";",4), fixed = TRUE))),ncol = 4)
basicCD_boundingSpheres_10 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_boundingSpheres_10[,1], ";",4), fixed = TRUE))),ncol = 4)
basicCD_boundingSpheres_100 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_boundingSpheres_100[,1], ";",4), fixed = TRUE))),ncol = 4)
basicCD_boundingSpheres_1000 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_boundingSpheres_1000[,1], ";",4), fixed = TRUE))),ncol = 4)
AABB_TD_median_10 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_AABB_TD_median_10[,1], ";",4), fixed = TRUE))),ncol = 4)
AABB_TD_median_100 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_AABB_TD_median_100[,1], ";",4), fixed = TRUE))),ncol = 4)
AABB_TD_median_1000 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_AABB_TD_median_1000[,1], ";",4), fixed = TRUE))),ncol = 4)
AABB_TD_SAH_10 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_AABB_TD_SAH_10[,1], ";",4), fixed = TRUE))),ncol = 4)
AABB_TD_SAH_100 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_AABB_TD_SAH_100[,1], ";",4), fixed = TRUE))),ncol = 4)
AABB_TD_SAH_1000 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_AABB_TD_SAH_1000[,1], ";",4), fixed = TRUE))),ncol = 4)
AABB_insertion_SAH_10 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_AABB_insertion_SAH_10[,1], ";",4), fixed = TRUE))),ncol = 4)
AABB_insertion_SAH_100 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_AABB_insertion_SAH_100[,1], ";",4), fixed = TRUE))),ncol = 4)
AABB_insertion_SAH_1000 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_AABB_insertion_SAH_1000[,1], ";",4), fixed = TRUE))),ncol = 4)

#generate average_time for each column
averages_basicCD_unoptimised_10 = apply(basicCD_unoptimised_10, 2, mean)[2:4]
averages_basicCD_unoptimised_100 = apply(basicCD_unoptimised_100, 2, mean)[2:4]
averages_basicCD_unoptimised_1000 = apply(basicCD_unoptimised_1000, 2, mean)[2:4]
averages_basicCD_SAT_10 = apply(basicCD_SAT_10, 2, mean)[2:4]
averages_basicCD_SAT_100 = apply(basicCD_SAT_100, 2, mean)[2:4]
averages_basicCD_SAT_1000 = apply(basicCD_SAT_1000, 2, mean)[2:4]
averages_basicCD_boundingSpheres_10 = apply(basicCD_boundingSpheres_10, 2, mean)[2:4]
averages_basicCD_boundingSpheres_100 = apply(basicCD_boundingSpheres_100, 2, mean)[2:4]
averages_basicCD_boundingSpheres_1000 = apply(basicCD_boundingSpheres_1000, 2, mean)[2:4]
averages_AABB_TD_median_10 = apply(AABB_TD_median_10, 2, mean)
averages_AABB_TD_median_100 = apply(AABB_TD_median_100, 2, mean)
averages_AABB_TD_median_1000 = apply(AABB_TD_median_1000, 2, mean)
averages_AABB_TD_SAH_10 = apply(AABB_TD_SAH_10, 2, mean)
averages_AABB_TD_SAH_100 = apply(AABB_TD_SAH_100, 2, mean)
averages_AABB_TD_SAH_1000 = apply(AABB_TD_SAH_1000, 2, mean)
averages_AABB_insertion_SAH_10 = apply(AABB_insertion_SAH_10, 2, mean)
averages_AABB_insertion_SAH_100 = apply(AABB_insertion_SAH_100, 2, mean)
averages_AABB_insertion_SAH_1000 = apply(AABB_insertion_SAH_1000, 2, mean)

#generate total_time for each row
totals_basicCD_unoptimised_10 = rowSums(basicCD_unoptimised_10)
totals_basicCD_unoptimised_100 = rowSums(basicCD_unoptimised_100)
totals_basicCD_unoptimised_1000 = rowSums(basicCD_unoptimised_1000)
totals_basicCD_SAT_10 = rowSums(basicCD_SAT_10)
totals_basicCD_SAT_100 = rowSums(basicCD_SAT_100)
totals_basicCD_SAT_1000 = rowSums(basicCD_SAT_1000)
totals_basicCD_boundingSpheres_10 = rowSums(basicCD_boundingSpheres_10)
totals_basicCD_boundingSpheres_100 = rowSums(basicCD_boundingSpheres_100)
totals_basicCD_boundingSpheres_1000 = rowSums(basicCD_boundingSpheres_1000)
totals_AABB_TD_median_10 = rowSums(AABB_TD_median_10)
totals_AABB_TD_median_100 = rowSums(AABB_TD_median_100)
totals_AABB_TD_median_1000 = rowSums(AABB_TD_median_1000)
totals_AABB_TD_SAH_10 = rowSums(AABB_TD_SAH_10)
totals_AABB_TD_SAH_100 = rowSums(AABB_TD_SAH_100)
totals_AABB_TD_SAH_1000 = rowSums(AABB_TD_SAH_1000)
totals_AABB_insertion_SAH_10 = rowSums(AABB_insertion_SAH_10)
totals_AABB_insertion_SAH_100 = rowSums(AABB_insertion_SAH_100)
totals_AABB_insertion_SAH_1000 = rowSums(AABB_insertion_SAH_1000)

#create and plot fitted lines for execution over time, in 3 graphs using n=10,100,1000
library(gcookbook)
library(ggplot2)
palette.colors(palette = "Okabe-Ito")
x1= (1:599)
yAll = (as.data.frame(cbind(totals_basicCD_boundingSpheres_1000, totals_AABB_TD_median_1000,totals_AABB_insertion_SAH_1000)))
data_ggp <- data.frame(x = x1,                            # Reshape data frame
                       y = c(totals_basicCD_boundingSpheres_1000, totals_AABB_TD_median_1000,totals_AABB_insertion_SAH_1000),
                       CD_algorithm = c(rep("BC pre-test", nrow(yAll)),
                                 rep("AABB TD median", nrow(yAll)),
                                 rep("AABB insertion SAH", nrow(yAll))))
p<-ggplot(data_ggp, aes(x=x, y=y, group=CD_algorithm, color=CD_algorithm)) + 
    geom_smooth(method = "lm", se = FALSE) + geom_point(alpha = 0.1)+
    scale_color_manual(values=c('#abd9e9', '#2c7bb6', '#d7191c', '#fdae61'))+
    ggtitle("Plot of total time per iteration for basic CD and AABB BVH, N = 1000") +
    ylab("total seconds/iteration") + xlab("iteration")
print(p)

#x2=row.names(yAll)
#ggplot(yAll, aes(x = x2, y = §), color=colname()) + geom_point(alpha = 0.1) + geom_smooth(method = "lm", se = FALSE)+
#geom_smooth(aes(y=totals_AABB_TD_median_1000),method = "lm", se = FALSE) + geom_point(aes(y=totals_AABB_TD_median_1000), alpha = 0.1)+
#geom_smooth(aes(y=totals_AABB_TD_SAH_1000),method = "lm", se = FALSE) + geom_point(aes(y=totals_AABB_TD_SAH_1000), alpha = 0.1)+
#geom_smooth(aes(y=totals_AABB_insertion_SAH_1000),method = "lm", se = FALSE) + geom_point(aes(y=totals_AABB_insertion_SAH_1000), alpha = 0.1)+
#ggtitle("Plot of total time per iteration for controller and AABB, N = 1000") +
#ylab("total seconds/iteration") + xlab("iteration")+ scale_color_manual(values=c('orange', 'pink', 'red', 'blue'))



#plot columns split into average times for each Build,CD,CH,Other for each plot, in 3 graphs using n=10,100,1000
basicCD_10 <- cbind(averages_basicCD_unoptimised_10, averages_basicCD_SAT_10, averages_basicCD_boundingSpheres_10)
basicCD_100 <- cbind(averages_basicCD_unoptimised_100, averages_basicCD_SAT_100, averages_basicCD_boundingSpheres_100)
basicCD_1000 <- cbind(averages_basicCD_unoptimised_1000, averages_basicCD_SAT_1000, averages_basicCD_boundingSpheres_1000)
colnames(basicCD_10) <- c("unoptimised", "SAT-pregeneration", "BC pre-test")
colnames(basicCD_100) <- c("unoptimised", "SAT-pregeneration", "BC pre-test")
colnames(basicCD_1000) <- c("unoptimised", "SAT-pregeneration", "BC pre-test")
rownames(basicCD_10) <- c("CD", "CH", "other")
rownames(basicCD_100) <- c("CD", "CH", "other")
rownames(basicCD_1000) <- c("CD", "CH", "other")
AABB_10 <- cbind(append(averages_basicCD_boundingSpheres_10, 0, 0), averages_AABB_TD_median_10, averages_AABB_TD_SAH_10, averages_AABB_insertion_SAH_10)
AABB_100 <- cbind(append(averages_basicCD_boundingSpheres_100, 0, 0), averages_AABB_TD_median_100, averages_AABB_TD_SAH_100, averages_AABB_insertion_SAH_100)
AABB_1000 <- cbind(append(averages_basicCD_boundingSpheres_1000, 0, 0), averages_AABB_TD_median_1000, averages_AABB_TD_SAH_1000, averages_AABB_insertion_SAH_1000)
colnames(AABB_10) <- c("BC pre-test", "TD median", "TD SAH", "insertion SAH")
colnames(AABB_100) <- c("BC pre-test", "TD median", "TD SAH", "insertion SAH")
colnames(AABB_1000) <- c("BC pre-test", "TD median", "TD SAH", "insertion SAH")
rownames(AABB_10) <- c("build", "CD", "CH", "other")
rownames(AABB_100) <- c("build", "CD", "CH", "other")
rownames(AABB_1000) <- c("build", "CD", "CH", "other")
all_10 <- cbind(append(averages_basicCD_unoptimised_10, 0, 0),append(averages_basicCD_SAT_10, 0, 0), append(averages_basicCD_boundingSpheres_10, 0, 0), averages_AABB_TD_median_10, averages_AABB_insertion_SAH_10)
all_100 <- cbind(append(averages_basicCD_unoptimised_100, 0, 0),append(averages_basicCD_SAT_100, 0, 0), append(averages_basicCD_boundingSpheres_100, 0, 0), averages_AABB_TD_median_100, averages_AABB_insertion_SAH_100)
all_1000 <- cbind(append(averages_basicCD_unoptimised_1000, 0, 0),append(averages_basicCD_SAT_1000, 0, 0), append(averages_basicCD_boundingSpheres_1000, 0, 0), averages_AABB_TD_median_1000, averages_AABB_insertion_SAH_1000)
colnames(all_10) <- c("unoptimised", "SAT-pregeneration", "BC pre-test", "TD median",  "insertion SAH")
colnames(all_100) <- c("unoptimised", "SAT-pregeneration", "BC pre-test", "TD median",  "insertion SAH")
colnames(all_1000) <- c("unoptimised", "SAT-pregeneration", "BC pre-test", "TD median",  "insertion SAH")
rownames(all_10) <- c("build", "CD", "CH", "other")
rownames(all_100) <- c("build", "CD", "CH", "other")
rownames(all_1000) <- c("build", "CD", "CH", "other")


# create table with times
library(gridExtra)
library(grid)
data <- data.frame(facet = c(rep(N, each = length(group_CD_algorithm)*length(stack_CD_algorithm))),
				group = group_CD_algorithm,
				stack = stack_CD_algorithm,
				value = c(AABB_10, AABB_100, AABB_1000))
tab<-table(data)
png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/test.png")
p<-tableGrob(data)
grid.arrange(p)
dev.off()

#plot columns split into average times for each Build,CD,CH,Other for each plot, in 3 categories using n=10,100,1000
# facet = N (10,100,1000)
# group = CD_algorithm_used
# value = seconds/iteration
# stack = time_categories
N <- c(10,100,1000)
group_CD_algorithm <- c("BC pre-test", "TD median", "TD SAH", "insertion SAH") #4
stack_CD_algorithm <- c("build", "CD", "CH", "other") #4
data <- data.frame(facet = c(rep(N, each = length(group_CD_algorithm)*length(stack_CD_algorithm))),
				group = group_CD_algorithm,
				stack = stack_CD_algorithm,
				value = c(AABB_10, AABB_100, AABB_1000))
ggplot(data,                         
       aes(x = group,
           y = value,
           fill = stack)) + 
  geom_bar(stat = "identity",
           position = "stack") +
  facet_grid(~ facet)



png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_all_10.png")
barplot(all_10*10^3, main = expression("Average execution times, N = 10"), sub = "",xlab = "CD-structure used", ylab = expression(10^-3 ~ "seconds/iteration"), axes = TRUE, legend.text = rownames(all_10), width = 1000)
print(g)
dev.off()
png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_all_100.png")
barplot(all_100*10^3, main = expression("Average execution times, N = 100"), sub = "",xlab = "CD-structure used", ylab = expression(10^-3 ~ "seconds/iteration"), axes = TRUE, legend.text = rownames(all_100))
print(g)
dev.off()
png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_all_1000.png")
barplot(all_1000*10^3, main = expression("Average execution times, N = 1000"), sub = "",xlab = "CD-structure used", ylab = expression(10^-3 ~ "seconds/iteration"), legend.text = rownames(all_1000))
print(g)
dev.off()

png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_controllers_10.png")
g <- barplot(controllers_10, main = expression(N^2 ~ "controllers, N = 10"), sub = "",xlab = "CD-structure used", ylab = "seconds/iteration", axes = TRUE, legend.text = rownames(controllers_10))
print(g)
dev.off()
png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_controllers_100.png")
g <- barplot(controllers_100, main = expression(N^2 ~ "controllers, N = 100"), sub = "",xlab = "CD-structure used", ylab = "seconds/iteration", axes = TRUE, legend.text = rownames(controllers_100))
print(g)
dev.off()
png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_controllers_1000.png")
g <- barplot(controllers_1000, main = expression(N^2 ~ "controllers, N = 1000"), sub = "",xlab = "CD-structure used", ylab = "seconds/iteration", axes = TRUE, legend.text = rownames(controllers_1000), args.legend = list(x = "topleft"))
print(g)
dev.off()
png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_AABB_10.png")
g <- barplot(AABB_10, main = expression(N^2 ~ "controller vs AABB BVH:s, N = 10"), sub = "",xlab = "CD-structure used", ylab = "seconds/iteration", axes = TRUE, legend.text = rownames(AABB_10), args.legend = list(x = "topleft"))
print(g)
dev.off()
png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_AABB_100.png")
g <- barplot(AABB_100, main = expression(N^2 ~ "controller vs AABB BVH:s, N = 100"), sub = "",xlab = "CD-structure used", ylab = "seconds/iteration", axes = TRUE, legend.text = rownames(AABB_100))
print(g)
dev.off()
png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_AABB_1000.png")
g <- barplot(AABB_1000, main = expression(N^2 ~ "controller vs AABB BVH:s, N = 1000"), sub = "",xlab = "CD-structure used", ylab = "seconds/iteration", axes = TRUE, legend.text = rownames(AABB_1000))
print(g)
dev.off()