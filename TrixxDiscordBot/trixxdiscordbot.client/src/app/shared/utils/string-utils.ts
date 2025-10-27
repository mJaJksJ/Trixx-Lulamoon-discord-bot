export const comareStrings = (str: string, compareStrs: string[]) => {
    const simplyStr = simplifyStr(str);
    console.log(simplyStr)
    for (const compareStr of compareStrs) {
        const simplyCompareStr = simplifyStr(compareStr);

        if (!simplyCompareStr) {
            return false;
        }

        if (simplyStr.includes(simplyCompareStr) || simplyCompareStr.includes(simplyStr)) {
            console.log([simplyStr, simplyCompareStr])
            return true;
        }
    }
    return false;
}

export const simplifyStr = (str: string) => {
    return str
        .replace(/\s+/g, '')
        .toLowerCase()
        .replace(/ё/g, 'е');
}