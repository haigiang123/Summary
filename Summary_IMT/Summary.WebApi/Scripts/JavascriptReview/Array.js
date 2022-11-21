
const arr1 = [1, "dog", "mouse", "duck"];
const arr2 = ["chicken", "hourse"];

// concat
const concat1 = arr1.concat(arr2);
console.log(concat1);

// copyWithin
const copyWithin1 = arr1.copyWithin(2, 0, 2);
console.log(copyWithin1);

// every
function CheckIsString(item) {
    return !parseInt(item);
}

const every1 = arr1.every(CheckIsString);
console.log(arr1);
console.log(every1);


