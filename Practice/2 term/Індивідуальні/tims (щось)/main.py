import matplotlib.pyplot as plt
from scipy.integrate import quad
import numpy as np
import math

def interval_fmapping(values_list, frequencies_list):
    samples_list = []
    for i in range(len(values_list)):
        for j in range(frequencies_list[i]):
            samples_list.append(values_list[i])

    log_val = math.log(sum(frequencies_list), 2)
    r = 1
    while not(log_val > r and log_val <= r + 1):
        r += 1
    step = (max(values_list) - min(values_list)) / r
    start_num = values_list[0]
    interval_dict = {}
    for i in range(r):
        interval_dict[(start_num, start_num + step)] = 0
        start_num += step
    samples_list.sort()
    intervals_list = list(interval_dict.keys())
    intervalsfreq_list = [0 for i in range(r)]
    iterator = 0
    while samples_list[iterator] >= intervals_list[0][0] and samples_list[iterator] <= intervals_list[0][1]:
        intervalsfreq_list[0] += 1
        iterator += 1

    for i in range(1, r):
        try:
            while samples_list[iterator] > intervals_list[i][0] and samples_list[iterator] <= intervals_list[i][1]:
                intervalsfreq_list[i] += 1
                iterator += 1
        except IndexError:
            pass
    for i in range(r):
        interval_dict[intervals_list[i]] = intervalsfreq_list[i]
    del intervals_list
    del intervalsfreq_list
    return interval_dict
def teo_probs(values_list, frequencies_list): #теоретичні ймовірності для закону розподілу Пуассона
    def integrand(x, a, b):
        return np.exp( -(((x - a)**2)/ 2*(b**2)))

    def F(x, a, s):
        result = (1 / s * math.sqrt(2 * math.pi)) * quad(integrand, -np.inf, x, args=(a, s))[0]
        return result

    mids_list = []
    for i in range(len(values_list)):
        mids_list.append((values_list[i][0] + values_list[i][1]) / 2)
    average = 0
    for i in range(len(values_list)):
        average += mids_list[i] * frequencies_list[i]
    average /= sum(frequencies_list)
    dev = 0
    for i in range(len(frequencies_list)):
        dev += frequencies_list[i] * ((mids_list[i] - average) ** 2)
    s = math.sqrt(dev / (sum(frequencies_list) - 1))
    probs_list = []
    for i in range(len(values_list)):
        probs_list.append(F(values_list[i][1], average, s) - F(values_list[i][0], average, s))
    return probs_list


def teo_probs2(values_list, frequencies_list): #теоретичні ймовірності для закону розподілу Пуассона
    lam = 0
    for i in range(len(values_list)):
        lam += values_list[i] * frequencies_list[i]
    lam /= sum(frequencies_list)
    probs_list = []
    for i in range(len(values_list)):
        probs_list.append( (math.e ** (-lam)) * (lam**i) / math.factorial(i))
    return probs_list
#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Задача 6
print("Задача 4")
file = open("data")
values_string = file.readline()
frequencies_string = file.readline()
values_list = values_string.split(' ')
frequencies_string = frequencies_string.split(' ')
frequencies_list = list(map(int, frequencies_string))
for i in range(len(values_list)):
    string_pair = values_list[i].split('-')
    int_tuple = tuple(map(int, string_pair))
    values_list[i] = int_tuple

mid_values = []
histogram_data = []
for i in values_list:
    mid_values.append((i[1] + i[0]) / 2)
for i in range(len(values_list)):
    for j in range(frequencies_list[i]):
        histogram_data.append(mid_values[i])
plt.hist(histogram_data)
plt.show()




print("H0 - законом розподілу випадкової величини є нормальний закон розподілу")
probs_list = teo_probs(values_list, frequencies_list)
Xi = 0
for i in range(len(values_list)):
    Xi += ((frequencies_list[i] - sum(frequencies_list) * probs_list[i]) ** 2) / sum(frequencies_list) * probs_list[i]
print(f"Xi = {Xi}")
choice = input("Виберіть рівень значущості:\n1. 0.99\n2. 0.975\n3. 0.95\n4. 0.9\n5. 0.1\n6. 0.05\n7. 0.025\n8. 0.01\n")
if choice == '1':
    significance_index = 0
elif choice == '2':
    significance_index = 1
elif choice == '3':
    significance_index = 2
elif choice == '4':
    significance_index = 3
elif choice == '5':
    significance_index = 4
elif choice == '6':
    significance_index = 5
elif choice == '7':
    significance_index = 6
elif choice == '8':
    significance_index = 7
else:
    significance_index = 0
X_list = [1.24, 1.69, 2.17, 2.83, 12.02, 14.07, 16.01, 18.48]
#k = 10 - 2 - 1 = 7
critical_point = X_list[significance_index]
if Xi >= critical_point:
    print("Гіпотезу H0 відхилено")
else:
    print("Гіпотезу H0 прийнято")



#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Задача 12
file = open("data2")
values_string = file.readline()
frequencies_string = file.readline()

values_list = values_string.split(' ')
values_list[-1] = values_list[-1][0:len(values_list[-1] ) - 1]
frequencies_list = frequencies_string.split(' ')
values_list = list(map(int, values_list))
frequencies_list = list(map(int, frequencies_list))

interval_dict = interval_fmapping(values_list, frequencies_list)
plt.plot(values_list, frequencies_list)
plt.show()
# H0 - закон розподілу нормальної величини є законом розподілу Пуассона


probs_list = teo_probs2(values_list, frequencies_list)
Xi = 0
for i in range(len(values_list)):
    Xi += ((frequencies_list[i] - sum(frequencies_list) * probs_list[i]) ** 2) / sum(frequencies_list) * probs_list[i]

print('Задача 12\nH0 - закон розподілу випадкової величини є закон розподілу Пуассона\n')
print("Xi = ", Xi)
choice = input("Виберіть рівень значущості:\n1. 0.99\n2. 0.975\n3. 0.95\n4. 0.9\n5. 0.1\n6. 0.05\n7. 0.025\n8. 0.01\n")
if choice == '1':
    significance_index = 0
elif choice == '2':
    significance_index = 1
elif choice == '3':
    significance_index = 2
elif choice == '4':
    significance_index = 3
elif choice == '5':
    significance_index = 4
elif choice == '6':
    significance_index = 5
elif choice == '7':
    significance_index = 6
elif choice == '8':
    significance_index = 7
else:
    significance_index = 0



# k = 6    (8 - 1 - 1)
X_list = [0.872, 1.24, 1.64, 2.2, 10.64, 12.59, 14.45, 16.81]
critical_point = X_list[significance_index]
if Xi >= critical_point:
    print("Гіпотезу H0 відхилено")
else:
    print("Гіпотезу H0 прийнято")



import random

def b_sort (s):
#bubble sort
