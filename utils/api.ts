import { Video } from "@/model/video";
import { faker } from "@faker-js/faker";

export const getRandomVideos = (number: number) => {
  const data: Video[] = [];
  const states: ("like" | "dislike" | null)[] = ["like", "dislike", null];
  for (let i = 0; i < number; i += 1) {
    const randomObj: Video = {
      id: faker.string.alphanumeric({ length: 25 }),
      name: faker.word.words({ count: Math.ceil(Math.random() * 10) }),
      description: faker.lorem.paragraph(),
      preview: faker.image.urlLoremFlickr(),
      date: faker.date.past().valueOf(),
      state: states[Math.floor(Math.random() * 3)],
    };

    data.push(randomObj);
  }

  return data;
};
