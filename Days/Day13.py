import sys
import functools
from functools import cmp_to_key

infile = "../inputs/Day13.txt" if len(sys.argv)<=1 or sys.argv[1] != "t" else "../inputs/Day13Test.txt"
packetPairs = open(infile, "r").read().split("\n\n")

def comparePackets(l1, l2):
    if isinstance(l1, int) and isinstance(l2, int):
        if l1 < l2:
            return -1
        elif l1 == l2:
            return 0
        else:
            return 1
    if isinstance(l1, list) and isinstance(l2, list):
        i = 0
        while i < len(l1) and i < len(l2):
            c = comparePackets(l1[i], l2[i])
            if c == 1:
                return 1
            elif c == -1:
                return -1
            i += 1
        if i == len(l1) and i < len(l2):
            return -1
        elif i < len(l1) and i == len(l2):
            return 1
        else:
            return 0
    elif isinstance(l1, int) and isinstance(l2, list):
        return comparePackets([l1], l2)
    else:  # isinstance(l1, list) and isinstance(l2, int):
        return comparePackets(l1, [l2])

packets = []
ans = 0
for i, pair in enumerate(packetPairs):
    l1, l2 = pair.split("\n")
    l1 = eval(l1)
    l2 = eval(l2)
    packets.append(l1)
    packets.append(l2)
    if comparePackets(l1, l2) == -1:
        ans += i + 1

print(ans)

packets.append([2])
packets.append([6])

packets = sorted(packets, key=functools.cmp_to_key(comparePackets))

ans2 = 1
for i, packet in enumerate(packets):
    if packet == [2] or packet == [6]:
        ans2 *= i + 1

print(ans2)
