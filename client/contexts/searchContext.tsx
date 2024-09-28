import { useDebounce } from "@uidotdev/usehooks";
import {
  ChangeEvent,
  ChangeEventHandler,
  createContext,
  FC,
  PropsWithChildren,
  useCallback,
  useContext,
  useState,
} from "react";

interface SearchContextType {
  search: string;
  onSearch: (e: ChangeEvent<HTMLInputElement> | string) => void;
}

const SearchContext = createContext<SearchContextType>({
  search: "",
  onSearch: () => {
    ///
  },
});

export const SearchProvider: FC<PropsWithChildren> = ({ children }) => {
  const [search, setSearch] = useState("");
  const searchDeb = useDebounce(search, 500);

  const onSearch = useCallback((e: ChangeEvent<HTMLInputElement> | string) => {
    if (typeof e === "string") setSearch(e);
    else setSearch(e.target.value);
  }, []);

  return <SearchContext.Provider value={{ search: searchDeb, onSearch }}>{children}</SearchContext.Provider>;
};

export const useSearchContext = () => {
  const cntx = useContext(SearchContext);

  return cntx;
};
