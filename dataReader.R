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
controller_unoptimised_10 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_unoptimised_10[,1], ";",4), fixed = TRUE))),ncol = 4)
controller_unoptimised_100 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_unoptimised_100[,1], ";",4), fixed = TRUE))),ncol = 4)
controller_unoptimised_1000 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_unoptimised_1000[,1], ";",4), fixed = TRUE))),ncol = 4)
controller_SAT_10 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_SAT_10[,1], ";",4), fixed = TRUE))),ncol = 4)
controller_SAT_100 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_SAT_100[,1], ";",4), fixed = TRUE))),ncol = 4)
controller_SAT_1000 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_SAT_1000[,1], ";",4), fixed = TRUE))),ncol = 4)
controller_boundingSpheres_10 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_boundingSpheres_10[,1], ";",4), fixed = TRUE))),ncol = 4)
controller_boundingSpheres_100 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_boundingSpheres_100[,1], ";",4), fixed = TRUE))),ncol = 4)
controller_boundingSpheres_1000 <- matrix(as.numeric(as.matrix(sub(",", ".",str_split_fixed(data_controller_boundingSpheres_1000[,1], ";",4), fixed = TRUE))),ncol = 4)
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
averages_controller_unoptimised_10 = apply(controller_unoptimised_10, 2, mean)[2:4]
averages_controller_unoptimised_100 = apply(controller_unoptimised_100, 2, mean)[2:4]
averages_controller_unoptimised_1000 = apply(controller_unoptimised_1000, 2, mean)[2:4]
averages_controller_SAT_10 = apply(controller_SAT_10, 2, mean)[2:4]
averages_controller_SAT_100 = apply(controller_SAT_100, 2, mean)[2:4]
averages_controller_SAT_1000 = apply(controller_SAT_1000, 2, mean)[2:4]
averages_controller_boundingSpheres_10 = apply(controller_boundingSpheres_10, 2, mean)[2:4]
averages_controller_boundingSpheres_100 = apply(controller_boundingSpheres_100, 2, mean)[2:4]
averages_controller_boundingSpheres_1000 = apply(controller_boundingSpheres_1000, 2, mean)[2:4]
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
totals_controller_unoptimised_10 = rowSums(controller_unoptimised_10)
totals_controller_unoptimised_100 = rowSums(controller_unoptimised_100)
totals_controller_unoptimised_1000 = rowSums(controller_unoptimised_1000)
totals_controller_SAT_10 = rowSums(controller_SAT_10)
totals_controller_SAT_100 = rowSums(controller_SAT_100)
totals_controller_SAT_1000 = rowSums(controller_SAT_1000)
totals_controller_boundingSpheres_10 = rowSums(controller_boundingSpheres_10)
totals_controller_boundingSpheres_100 = rowSums(controller_boundingSpheres_100)
totals_controller_boundingSpheres_1000 = rowSums(controller_boundingSpheres_1000)
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
x1= (1:599)
y1=(unlist(as.data.frame(totals_controller_unoptimised_10)))
y2=unlist(as.data.frame(totals_controller_boundingSpheres_10))
line <- lm(y1 ~ ., data=as.data.frame(totals_controller_unoptimised_10))
smoothingSpline1 = smooth.spline(x1, y1, spar=0.35)
smoothingSpline2 = smooth.spline(x1, y2, spar=0.35)
plot(x1, y1[1:599], type='n')
lines(line)
yAll = (unlist(as.data.frame({totals_controller_unoptimised_10, totals_controller_unoptimised_100)))

ggplot(as.data.frame(totals_controller_unoptimised_10), aes(x = x1, y = y1) ) + geom_point() + geom_smooth(method = "lm", se = FALSE)


lines(smoothingSpline1)
lines(smoothingSpline2)
plot(x,y1)
plot(x,y2)
line <- lm(y ~ x, data=as.data.frame(totals_controller_unoptimised_10))
ggplot(as.data.frame(totals_controller_unoptimised_10), aes(x = x1, y = resp, color = grp) ) + geom_point() + geom_smooth(method = "lm", se = FALSE)


#plot columns split into average times for each Build,CD,CH,Other for each plot, in 3 graphs using n=10,100,1000
all_1000 <- cbind(averages_controller_unoptimised_1000, averages_controller_SAT_1000, averages_controller_boundingSpheres_1000, averages_AABB_TD_median_1000, averages_AABB_TD_SAH_1000, averages_AABB_insertion_SAH_1000)
controllers_10 <- cbind(averages_controller_unoptimised_10, averages_controller_SAT_10, averages_controller_boundingSpheres_10)
controllers_100 <- cbind(averages_controller_unoptimised_100, averages_controller_SAT_100, averages_controller_boundingSpheres_100)
controllers_1000 <- cbind(averages_controller_unoptimised_1000, averages_controller_SAT_1000, averages_controller_boundingSpheres_1000)
colnames(controllers_10) <- c("unoptimised", "SAT-pregeneration", "BC pre-test")
colnames(controllers_100) <- c("unoptimised", "SAT-pregeneration", "BC pre-test")
colnames(controllers_1000) <- c("unoptimised", "SAT-pregeneration", "BC pre-test")
rownames(controllers_10) <- c("collission detection", "collission handling", "other")
rownames(controllers_100) <- c("collission detection", "collission handling", "other")
rownames(controllers_1000) <- c("collission detection", "collission handling", "other")
AABB_10 <- cbind(append(averages_controller_boundingSpheres_10, 0, 0), averages_AABB_TD_median_10, averages_AABB_TD_SAH_10, averages_AABB_insertion_SAH_10)
AABB_100 <- cbind(append(averages_controller_boundingSpheres_100, 0, 0), averages_AABB_TD_median_100, averages_AABB_TD_SAH_100, averages_AABB_insertion_SAH_100)
AABB_1000 <- cbind(append(averages_controller_boundingSpheres_1000, 0, 0), averages_AABB_TD_median_1000, averages_AABB_TD_SAH_1000, averages_AABB_insertion_SAH_1000)
colnames(AABB_10) <- c("BC pre-test", "TD median", "TD SAH", "insertion SAH")
colnames(AABB_100) <- c("BC pre-test", "TD median", "TD SAH", "insertion SAH")
colnames(AABB_1000) <- c("BC pre-test", "TD median", "TD SAH", "insertion SAH")
rownames(AABB_10) <- c("tree build", "collission detection", "collission handling", "other")
rownames(AABB_100) <- c("tree build", "collission detection", "collission handling", "other")
rownames(AABB_1000) <- c("tree build", "collission detection", "collission handling", "other")


png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_controllers_10.png")
g <- barplot(controllers_10, main = expression(N^2 ~ "controllers, N = 10"), sub = "",xlab = "CD-structure used", ylab = "seconds/iteration", axes = TRUE, legend.text = rownames(controllers_10))
print(g)
dev.off()
png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_controllers_100.png")
g <- barplot(controllers_100, main = expression(N^2 ~ "controllers, N = 100"), sub = "",xlab = "CD-structure used", ylab = "seconds/iteration", axes = TRUE, legend.text = rownames(controllers_100))
print(g)
dev.off()
png(file="C:/Users/Skuta/Documents/kandidatarbete/grafer/bars_controllers_1000.png")
g <- barplot(controllers_1000, main = expression(N^2 ~ "controllers, N = 1000"), sub = "",xlab = "CD-structure used", ylab = "seconds/iteration", axes = TRUE, legend.text = rownames(controllers_1000))
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















