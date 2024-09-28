export const beautifyDuration = (duration: number) => {
  const hours = Math.floor(duration / 3600);
  const minutes = Math.floor((duration - hours * 3600) / 60);
  const seconds = duration - hours * 3600 - minutes * 60;

  let result = "";
  if (seconds) result += `${seconds}c`;
  if (minutes) result = `${minutes}м ${result}`;
  if (hours) result = `${hours}ч ${result}`;

  return result;
};
